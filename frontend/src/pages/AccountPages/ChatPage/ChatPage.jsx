import "./ChatPage.css";
import React, { useState, useEffect } from 'react';
import {axiosToLitres} from "./../../../hooks/useAxios.js";
import {toast} from 'react-toastify';
import {ChatPreview} from "../../../components/UI/ChatPreview/ChatPreview.jsx";
import {Chat} from "../../../components/Chat/Chat.jsx";

//TODO: доделать
//TODO: убрать повторение кода, вынести повторяющиеся методы в отдельный компонет
export default function ChatPage() {
    const [chats, setChats] = useState([]);


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
    }, []);

    async function handleChatClick(userId) {
        await fetchUserChatMessages(userId);
    }

    return (
        <div className="chat-page">
            <div className="chat-page-preview">

                {chats.length ?
                    chats.map((chat, index) => (
                        <a key={index} onClick={async () => await handleChatClick(chat.userId)}>
                    <ChatPreview  lastMessageDate={chat.lastMessageDate} userId={chat.userId} username={chat.username}>
                    </ChatPreview> </a> )):
                    <p style={{textAlign: "center", fontWeight: 'bold'}}> No chats</p>
                    }
            </div>

            <div className="chat-page-container">
                    <Chat chatWindowStyle={{width: "80%"}} chatInputStyle={{width:"75%"}}
                          style={{display:"flex",flexDirection:"column",alignItems: "center",width:"100%"}}></Chat>
            </div>
        </div>
    );    
}