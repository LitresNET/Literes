import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import './Chat.css'; // Добавьте стили для вашего чата
import ICONS from "../../assets/icons.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";
import {Input} from "../../components/UI/Input/Input.jsx";
import {toast} from "react-toastify";
import {axiosToLitres} from "../../hooks/useAxios.js";

//TODO: добавить toast уведомление при новом сообщении
const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [connectionEstablished, setConnectionEstablished] = useState(false);
    const [messages, setMessages] = useState([
        // test messages
        {user: 'admin', message: 'fuck'},
        {user: 'admin', message: 'fuck'},
        {user: 'admin', message: 'fucksdakdsl;adksa;ldksa;dksadkakdassdaskdjasdjaskdjajkd'},
        {user: 'admin', message: 'fuck'},
        {user: 'me', message: 'ok'},
        {user: 'me', message: 'ok'}
    ]);
    const [message, setMessage] = useState('');
    const [isOpen, setIsOpen] = useState(false);

    useEffect(() => {
        const fetchChatData = async () => {
            try {
                const sessionId = localStorage.getItem('chatSessionId')
                const response = await axiosToLitres.get(`/chat/${sessionId}`);
                console.log('response: ', response)
                setMessages([...response.map(m => {user: m.from; m: m.text})])
            } catch (error) {
                console.log('error: ', error)
            }
        };
        fetchChatData()
        .then(result => {
            console.log('history result: ', result)
        })

        console.log('Connecting to ChatHub...');
        if (connection) {
            console.log('oops, connection already established, use this chat id: ', localStorage.getItem('chatSessionId'))
        }
        const token = localStorage.getItem('token')
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5225/api/hubs/chat', { 
                accessTokenFactory: () => token
            })
            .withAutomaticReconnect()
            .build();

        newConnection.on('ReceiveMessage', (message) => {
            console.log('new message!', message)
            setMessages((prev) => [...prev, {message: message.text, user: message.from}]);
        });

        newConnection.on('SetSessionId', (sessionId) => {
            localStorage.setItem('chatSessionId', sessionId);
        });

        if (!connectionEstablished) {
            newConnection.start()
            .then(()=>{
                setConnection(newConnection);
                setConnectionEstablished(true)
                console.log('established successfully')
            })
            .catch(e => {
                console.log('error while connecting: ', e)
            });
        }

        return () => {
            if (connection) {
                connection.stop();
            }
        };

    }, []);

    const makeScrollDown = () =>
    {
        let chat = document.getElementsByClassName("chat-messages")[0];
        chat.scrollTop = chat.scrollHeight
    }
    useEffect(() => {
        if (isOpen)
            makeScrollDown();
    }, [messages, isOpen]);

    const sendMessage = async () => {
        if (connection && message) {
            const newMessage = {
                ChatSessionId: localStorage.getItem('chatSessionId'),
                Text: message,
                From: 'User'
            };

            await connection.invoke('SendMessageAsync', newMessage);
            console.log('successfully sent a message')
            setMessage('');
            setMessages((prevMessages) => [...prevMessages, { user: "me", message }])
        }
        else {
            toast.error("chat don't working")
        }
    };

    const toggleChat = () => {
        setIsOpen(!isOpen);
    };



    return (
        <div className={"chat-container" + (isOpen ? " open" : "")}>
            {isOpen ? (
                <div>
                    <Button onClick={toggleChat} iconPath={ICONS.caret_down} className="close-button"></Button>
                    <div className="chat-messages">
                        {
                            messages.length ?
                            messages.map((msg, index) => (
                            <div className={"message"} key={index}>
                                <strong>{msg.user}: </strong>
                                <p>{msg.message}</p>
                            </div>
                        )) : <p style={{textAlign: "center", fontWeight: 'bold'}}>Got a question? Write to us!</p>}
                    </div>
                    <div className="chat-input">
                        <Input
                            type="text"
                            value={message}
                            onChange={(e) => setMessage(e.target.value)}
                            placeholder="Write message"
                        />
                        <Button onClick={sendMessage} text="Send"></Button>
                    </div>
                </div>
            ) : (
                <Button iconPath={ICONS.message} className="open-button" onClick={toggleChat}></Button>
            )}
        </div>
    );
};

export default Chat;
