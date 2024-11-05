import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import './Chat.css'; // Добавьте стили для вашего чата
import ICONS from "../../assets/icons.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";
import {Input} from "../../components/UI/Input/Input.jsx";

const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([
        {user: 'admin', message: 'fuck'}
    ]);
    const [message, setMessage] = useState('');
    const [isOpen, setIsOpen] = useState(false);

    useEffect(() => {
        const connect = async () => {
            const connection = new HubConnectionBuilder()
                .withUrl('https://your-signalr-server/chatHub') // URL на сервер SignalR
                .withAutomaticReconnect()
                .build();

            connection.on('ReceiveMessage', (user, message) => {
                setMessages((prevMessages) => [...prevMessages, { user, message }]);
            });

            await connection.start();
            setConnection(connection);
        };

        connect();

        return () => {
            if (connection) {
                connection.stop();
            }
        };
    }, []);

    const sendMessage = async () => {
        if (connection && message) {
            await connection.invoke('SendMessage', 'User  ', message);
            setMessage('');
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
