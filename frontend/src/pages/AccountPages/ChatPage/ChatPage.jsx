import "./ChatPage.css";
import { ChatWindow } from "../../../components/UI/ChatMessagesContainer/ChatWindow.jsx";
import { HubConnectionBuilder } from '@microsoft/signalr';
import React, { useState, useEffect } from 'react';
import {axiosToLitres} from "./../../../hooks/useAxios.js";
import {toast} from 'react-toastify';
import {Button} from "../../../components/UI/Button/Button.jsx";
import {Input} from "../../../components/UI/Input/Input.jsx";
import {ChatPreview} from "../../../components/UI/ChatPreview/ChatPreview.jsx";

//TODO: доделать
//TODO: убрать повторение кода, вынести повторяющиеся методы в отдельный компонет
export default function ChatPage() {
    const [connection, setConnection] = useState(null);
    const [connectionEstablished, setConnectionEstablished] = useState(false);
    const [chats, setChats] = useState([
        { userId: "124", username: "gitler", lastMessageDate: new Date() }
    ]);
    const [messages, setMessages] = useState([
        {from: 'admin', message: 'fuck', sentDate: new Date(1,2,3,4,5,6)},
        {from: 'admin', message: 'fuck', sentDate: '1.1.1'},
        {from: 'admin', message: 'fucksdakdsl;adksa;ldksa;dksadkakdassdaskdjasdjaskdjajkd', sentDate: '1.1.1'},
        {from: 'admin', message: 'fuck', sentDate: '1.1.1'},
        {from: 'me', message: 'ok', sentDate: '1.1.1'},
        {from: 'FuckImDead', message: 'ok', sentDate: '1.1.1'},
        {from: 'admin', message: 'fuck', sentDate: new Date(1,2,3,4,5,6)},
        {from: 'admin', message: 'fuck', sentDate: '1.1.1'},
        {from: 'admin', message: 'fucksdakdsl;adksa;ldksa;dksadkakdassdaskdjasdjaskdjajkdвыфлвоыфдлывовыфолдвфолдыврдфвдлоыфовлфыолврлоыфврлофрвлофырврыфлвофрлворфыловролвырфолврфлврфыл', sentDate: '1.1.1'},
        {from: 'admin', message: 'fuck', sentDate: '1.1.1'},
        {from: 'me', message: 'ok', sentDate: '1.1.1'},
        {from: 'FuckImDead', message: 'ok', sentDate: '1.1.1'}
    ]);
    const [message, setMessage] = useState('');

    const fetchAllChats = async () => {
        try {
            const response = await axiosToLitres.get('/chat/agent-chats');
            setChats([...response.data.map(m => ({ userId: m.userId, username: m.username, lastMessageDate: m.lastMessageDate }))])
        } catch (error) {
            toast.error(`Chat Page: ${error}`, { toastId: "ChatPageFetchError" })
        }
    };

    const fetchUserChatMessages = async (userId) => {
        try {
            const response = await axiosToLitres.get(`/chat/history/${userId}`);
            if (response.data.isSuccess) {
                setMessages([...response.data.messages.map(m => ({ from: m.from, message: m.text, sentDate: m.sentDate }))])
            }
        } catch (error) {
            toast.error(`Chat Page: ${error}`, { toastId: "ChatPageHistoryFetchError" })
        }
    };

    useEffect( () => {
        fetchAllChats();
        const token = localStorage.getItem('token')
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5225/api/hubs/chat', {
                accessTokenFactory: () => token
            })
            .withAutomaticReconnect()
            .build();

        newConnection.on('ReceiveMessage', (message) => {
            toast.success('Chat Page: New Message', {toastId: 'ChatPageNewMessage', autoClose: false})
            setMessages((prev) => [...prev, { message: message.text, user: message.from }]);
        });

        if (!connectionEstablished) {
            newConnection.start()
                .then(() => {
                    setConnection(newConnection);
                    setConnectionEstablished(true)
                })
                .catch(e => {
                    toast.error(`Chat: Error while connecting (${e})`, {toastId: "ChatPageConnectionError"})
                });
        }

        return () => {
            if (connection) {
                connection.stop();
            }
        };

    }, [connectionEstablished, connection]);

    function handleChatClick(userId) {
        fetchUserChatMessages(userId);
    }

    const handleKeyPress = async (event) => {
        if (event.key === 'Enter')
            await sendMessage();
    }

    const sendMessage = async () => {
        if (!message) {
            toast.error("Chat Page: Message cannot be empty", { toastId: "ChatEmptyMessage" })
            return
        }
        if (connection) {
            const newMessage = {
                Text: message,
                From: localStorage.getItem("username")
            };

            await connection.invoke('SendMessage', newMessage).then(() => {
                setMessage('');
                setMessages((prevMessages) => [...prevMessages, {
                    from: localStorage.getItem("username"), message: message, sentDate: new Date()
                }])
            }).catch((e) => toast.error(`Chat Page: Sending message error: ${e.message}`,
                { toastId: "ChatPageSendMessageError" }));

        } else {
            toast.error("Chat Page: No connection", { toastId: "ChatPageSendMessageError" });
        }
    };

    return (
        <div className="chat-page">
            <div className="chat-page-preview">

                {chats.length ?
                    chats.map((chat, index) => (
                        <a onClick={() => handleChatClick(chat.userId)}>
                    <ChatPreview key={index}  lastMessageDate={chat.lastMessageDate} userId={chat.userId} username={chat.username}>
                    </ChatPreview> </a> )):
                    <p style={{textAlign: "center", fontWeight: 'bold'}}> No chats</p>
                    }
            </div>

            <div className="chat-page-container">
                <ChatWindow messages={messages} style={{width: "80%"}}></ChatWindow>
                {/*style из chat.css*/}
                <div className="chat-input" style={{width:"75%"}}>
                    <Input
                        type="text"
                        value={message}
                        onChange={(e) => setMessage(e.target.value)}
                        placeholder="Write message"
                        onKeyDown={handleKeyPress}
                    />
                    <Button onClick={sendMessage} text="Send" disabled={!message}></Button>
                </div>
            </div>
        </div>
    );    
}