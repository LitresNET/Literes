import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import './Chat.css'; // Добавьте стили для вашего чата
import ICONS from "../../assets/icons.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";
import {Input} from "../../components/UI/Input/Input.jsx";
import {toast} from "react-toastify";

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

<<<<<<< HEAD
        newConnection.on('ReceiveMessage', (message) => {
            console.log('new message!', message)
            setMessages((prev) => [...prev, message]);
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

=======
            connection.on('ReceiveMessage', (user, message) => {
                setMessages((prevMessages) => [...prevMessages, { user, message }])
            });

            await connection.start();
            setConnection(connection);
        };

        connect();
>>>>>>> e07b904b57d014e2f3cba119861fea59a943e3f6
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
<<<<<<< HEAD
        if (connection && message) {
            const newMessage = {
                ChatSessionId: localStorage.getItem('chatSessionId'),
                Text: message,
                From: 'User'
            };

            await connection.invoke('SendMessageAsync', newMessage);
            console.log('successfully sent a message')
            setMessage('');
            setMessages((prev) => [...prev, {user: 'You', message: message}]);
=======
        //TODO: закомментирован псевдофункционал для теста фронта
        if (// connection &&
             message) {
          //  await connection.invoke('SendMessage', 'user', message);
            setMessage('');
            setMessages((prevMessages) => [...prevMessages, { user: "me", message }])
        }
        else {
            toast.error("chat don't working")
>>>>>>> e07b904b57d014e2f3cba119861fea59a943e3f6
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
