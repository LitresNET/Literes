import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [messageText, setMessageText] = useState('');

    useEffect(() => {
        console.log('Connecting to ChatHub...');
        const connect = async () => {
            const token = localStorage.getItem('token')
            const newConnection = new HubConnectionBuilder()
                .withUrl('http://localhost:5225/api/hubs/chat', {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                })
                .withAutomaticReconnect()
                .build();

            newConnection.on('ReceiveMessage', (message) => {
                setMessages((prev) => [...prev, message]);
            });

            newConnection.on('SetSessionId', (sessionId) => {
                console.log('Received sessionId:', sessionId);
                setChatSessionId(sessionId);
                localStorage.setItem('chatSessionId', sessionId);
            });

            try {
                await newConnection.start();
                console.log('Connected to ChatHub');
                setConnection(newConnection);
            } catch (err) {
                console.error('Error connecting to ChatHub:', err);
            }
        };

        connect();

        return () => {
            if (connection) {
                connection.stop();
            }
        };
    }, [connection]);

    const sendMessage = async () => {
        if (connection && messageText) {
            const message = {
                ChatSessionId: localStorage.getItem('chatSessionId'),
                Text: messageText,
                From: 'User'
            };

            await connection.invoke('SendMessageAsync', message);
            setMessageText('');
        }
    };

    return (
        <div>
            <input
                type="text"
                value={messageText}
                onChange={(e) => setMessageText(e.target.value)}
                placeholder="Enter message"
            />
            <button onClick={sendMessage}>Send</button>
            <div>
                {messages.map((msg, index) => (
                    <div key={index}>{msg.Text}</div>
                ))}
            </div>
        </div>
    );
};

export default Chat;