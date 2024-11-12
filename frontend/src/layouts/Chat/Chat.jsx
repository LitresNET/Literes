import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import './Chat.css';
import ICONS from "../../assets/icons.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";
import {Input} from "../../components/UI/Input/Input.jsx";
import {toast} from "react-toastify";
import {axiosToLitres} from "../../hooks/useAxios.js";
import {ChatMessage} from "../../components/UI/ChatMessage/ChatMessage.jsx";

//TODO: добавить toast уведомление при новом сообщении
const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [connectionEstablished, setConnectionEstablished] = useState(false);
    const [messages, setMessages] = useState([
        // test messages
        {user: 'admin', message: 'fuck', timestamp: '1:1:1'},
        {user: 'admin', message: 'fuck', timestamp: '1.1.1'},
        {user: 'admin', message: 'fucksdakdsl;adksa;ldksa;dksadkakdassdaskdjasdjaskdjajkd', timestamp: '1.1.1'},
        {user: 'admin', message: 'fuck', timestamp: '1.1.1'},
        {user: 'me', message: 'ok', timestamp: '1.1.1'},
        {user: 'FuckImDead', message: 'ok', timestamp: '1.1.1'}
    ]);
    const [message, setMessage] = useState('');
    const [isOpen, setIsOpen] = useState(false);

    useEffect(() => {
        //TODO: уберите пожалуйста логирование в консоль или замените их toast-уведомленими там, где надо
        //TODO: сделайте рабочий вариантик пж
        const fetchChatData = async () => {
            try {
                const sessionId = localStorage.getItem('chatSessionId')
                const response = await axiosToLitres.get(`/chat/${sessionId}`);
                console.log('response: ', response)
                setMessages([...response.map(m => {user: m.from; message: m.text; timestamp: m.sentDate})])
            } catch (error) {
                toast.error(`Chat: ${error}`, {toastId: "ChatFetchError"})
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
                toast.error(`Chat: error while connecting (${e})`)
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
        if (!message) {
            toast.error("Chat: Message cannot be empty", {toastId: "ChatEmptyMessage"})
            return
        }
        if (connection) {
            const newMessage = {
                ChatSessionId: localStorage.getItem('chatSessionId'),
                Text: message,
                From: 'User'
            };

            await connection.invoke('SendMessageAsync', newMessage).finally(() => {
                setMessage('');
                setMessages((prevMessages) => [...prevMessages, {
                    user: localStorage.getItem("Username"), message: message, timestamp: new Date() }])
            }).catch((e) => toast.error(`Chat: Sending message error: ${e.message}`),
                {toastId: "ChatSendMessageError"});

        }
        else {
            toast.error("Chat: No connection", {toastId: "ChatSendMessageError"});
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
                                       //TODO: заменить на компонент ChatWindow
                    <div className="chat-messages">
                        {
                            messages.length ?
                            messages.map((msg, index) =>
                                <ChatMessage key={index} text={msg.message} sender={msg.user} timestamp={msg.timestamp}></ChatMessage>)
                                : (<p style={{textAlign: "center", fontWeight: 'bold'}}>Got a question? Write to us!</p>)}
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
