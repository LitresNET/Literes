import "./ChatPage.css";
import { ChatWindow } from "../../../components/UI/ChatMessagesContainer/ChatWindow.jsx";
import { HubConnectionBuilder } from '@microsoft/signalr';
import { useState, useEffect } from 'react';
import {axiosToLitres} from "./../../../hooks/useAxios.js";
import {toast} from 'react-toastify'; 

//TODO: доделать
export default function ChatPage() {
    const [connection, setConnection] = useState(null);
    const [connectionEstablished, setConnectionEstablished] = useState(false);
    const [chats, setChats] = useState([]);
    const [messages, setMessages] = useState([]);
    const [message, setMessage] = useState('');

    const fetchAllChats = async () => {
        try {
            const response = await axiosToLitres.get('/chat/agent-chats');
            setChats([...response.data.map(m => ({ userId: m.userId, username: m.userName, timestamp: m.lastMessageDate }))])
        } catch (error) {
            toast.error(`Chats: ${error}`, { toastId: "ChatsFetchError" })
        }
    };

    const fetchUserChatMessages = async (userId) => {
        try {
            const response = await axiosToLitres.get(`/chat/history/${userId}`);
            if (response.data.isSuccess) {
                setMessages([...response.data.messages.map(m => ({ user: m.from, message: m.text, timestamp: m.sentDate }))])
            }
        } catch (error) {
            toast.error(`Messages: ${error}`, { toastId: "HistoryFetchError" })
        }
    };

    useEffect(() => {
        fetchAllChats();
        const token = localStorage.getItem('token')
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5225/api/hubs/chat', {
                accessTokenFactory: () => token
            })
            .withAutomaticReconnect()
            .build();

        newConnection.on('ReceiveMessage', (message) => {
            toast.success('New Message', {toastId: 'NewMessage'})
            setMessages((prev) => [...prev, { message: message.text, user: message.from }]);
        });

        if (!connectionEstablished) {
            newConnection.start()
                .then(() => {
                    setConnection(newConnection);
                    setConnectionEstablished(true)
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

    }, [connectionEstablished, connection]);

    const handleChatClick = (chat) => {
        fetchUserChatMessages(chat.userId);
    };

    const sendMessage = async () => {
        if (!message) {
            toast.error("Chat: Message cannot be empty", { toastId: "ChatEmptyMessage" })
            return
        }
        if (connection) {
            const newMessage = {
                Text: message,
                From: 'Admin'
            };

            await connection.invoke('SendMessage', newMessage).finally(() => {
                setMessage('');
                setMessages((prevMessages) => [...prevMessages, {
                    user: "Admin", message: message, timestamp: new Date()
                }])
            }).catch((e) => toast.error(`Chat: Sending message error: ${e.message}`),
                { toastId: "ChatSendMessageError" });

        } else {
            toast.error("Chat: No connection", { toastId: "ChatSendMessageError" });
        }
    };

    return (
        <div className="chat-page">
    
            <div className="chat-preview">
                {chats.map((chat) => (
                    <button
                        key={chat.userId}
                        className="chat-preview-item"
                        onClick={() => handleChatClick(chat)}
                    >
                        {chat.username}
                    </button>
                ))}
            </div>
    
            <div style={{display:"flex", flexDirection: "column"}}>
            <div className="chat-window">
                {messages.length === 0 ? (
                    <div>Чат пуст</div>
                ) : (
                    <div>
                        {messages.map((msg, index) => (
                            <div key={index}>
                                <div><strong>{msg.user}</strong></div>
                                <div>{msg.message}</div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
            <div className="chat-input">
                <input
                    type="text"
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    placeholder="Введите сообщение"
                />
                <button
                    onClick={sendMessage}
                    disabled={!message}
                >
                    Send
                </button>
            </div>
            </div>
        </div>
    );    
}