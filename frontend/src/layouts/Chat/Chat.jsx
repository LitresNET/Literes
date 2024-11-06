import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import './Chat.css'; // Добавьте стили для вашего чата
import ICONS from "../../assets/icons.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";
import {Input} from "../../components/UI/Input/Input.jsx";

const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [connectionEstablished, setConnectionEstablished] = useState(false);
    const [messages, setMessages] = useState([
        {user: 'admin', message: 'fuck'}
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

        return () => {
            if (connection) {
                connection.stop();
            }
        };
    }, []);

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
            setMessages((prev) => [...prev, {user: 'You', message: message}]);
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
                        {messages.map((msg, index) => (
                            <div key={index}>
                                <strong>{msg.user}: </strong>{msg.message}
                            </div>
                        ))}
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
