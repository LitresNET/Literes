import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import './Chat.css'; // Добавьте стили для вашего чата
import ICONS from "../../assets/icons.jsx";
import {Icon} from "../../components/UI/Icon/Icon.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";

const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
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
        <div>
            {isOpen ? (
                <div className="chat-container">
                    <div className="chat-messages">
                        {messages.map((msg, index) => (
                            <div key={index}>
                                <strong>{msg.user}: </strong>{msg.message}
                            </div>
                        ))}
                    </div>
                    <input
                        type="text"
                        value={message}
                        onChange={(e) => setMessage(e.target.value)}
                        placeholder="Введите сообщение"
                    />
                    <button onClick={sendMessage}>Отправить</button>
                    <button onClick={toggleChat} className="close-button">Закрыть</button>
                </div>
            ) : (
                <div className="chat-icon" onClick={toggleChat}>
                    <Icon path={ICONS.message} width={50} alt="Chat Icon" />
                </div>
            )}
        </div>
    );
};

export default Chat;
