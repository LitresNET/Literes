import {ChatWindow} from "../UI/ChatWindow/ChatWindow.jsx";
import React, {useEffect, useState} from "react";
import {ChatInput} from "../UI/ChatInput/ChatInput.jsx";
import {HubConnectionBuilder} from "@microsoft/signalr";
import {toast} from "react-toastify";
import {axiosToLitres} from "../../hooks/useAxios.js";
import './Chat.css'
import PropTypes from "prop-types";

export function Chat({chatWindowStyle, chatInputStyle, textIfEmpty, ...rest}) {
    Chat.propTypes ={
        textIfEmpty: PropTypes.string
    }

    const [connection, setConnection] = useState(null);
    const [connectionEstablished, setConnectionEstablished] = useState(false);
    const [messages, setMessages] = useState([]);

    const fetchChatData = async () => {
        try {
            const response = await axiosToLitres.get('/chat/history');

            setMessages([...response.data.messages?.map(m => ({from: m.from, message: m.text, sentDate: m.sentDate,
                fileModel: m.fileModel}))])
        } catch (error) {
            toast.error(`Chat: ${error}`, {toastId: "ChatFetchError"})
        }
    };

    useEffect( () => {
        if (connectionEstablished) return;
        const token = localStorage.getItem('token')
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5225/api/hubs/chat', {
                accessTokenFactory: () => token
            })
            .withAutomaticReconnect()
            .build();

        newConnection.on('ReceiveMessage', (message) => {
            toast.info('Chat: New Message', {toastId: 'ChatNewMessage', autoClose: false})
            /*
            if (chats.find(c => c.userId === message.chat.userId) === -1) {
                setChats((prev) => [...prev, { userId: message.chat.userId, lastMessageDate: message.sentDate }]);
            } */
            setMessages((prev) => [...prev, {from: message.from, message: message.text,
                sentDate:new Date(message.sentDate).toLocaleTimeString(), fileModel: message.fileModel}]);
        });

        if (!connectionEstablished) {
            newConnection.start()
                .then(()=>{
                    setConnection(newConnection);
                    setConnectionEstablished(true)
                })
                .catch(e => {
                    toast.error(`Chat: Error while connecting (${e})`, {toastId: "ChatConnectionError"})
                });
        }
        fetchChatData()

        return () => {
            if (connection) {
                connection.stop();
            }
        };

    }, []);

    return (
        <div className="chat" {...rest}>
            <ChatWindow
                messages={messages}
                textIfEmpty={textIfEmpty}
                style={chatWindowStyle}>
            </ChatWindow>
            <ChatInput connection={connection} setMessages={setMessages} style={chatInputStyle}></ChatInput>
        </div>
    );

}