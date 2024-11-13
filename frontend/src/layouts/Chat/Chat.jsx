import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import './Chat.css';
import ICONS from "../../assets/icons.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";
import {Input} from "../../components/UI/Input/Input.jsx";
import {toast} from "react-toastify";
import {axiosToLitres} from "./../../hooks/useAxios.js";
import {ChatWindow} from "../../components/UI/ChatMessagesContainer/ChatWindow.jsx";

//TODO: добавить toast уведомление при новом сообщении
const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [connectionEstablished, setConnectionEstablished] = useState(false);
    const [messages, setMessages] = useState([{from: 'admin', message: 'fuck', sentDate: new Date(1,2,3,4,5,6)},
        {from: 'admin', message: 'fuck', sentDate: '1.1.1'},
        {from: 'admin', message: 'fucksdakdsl;adksa;ldksa;dksadkakdassdaskdjasdjaskdjajkd', sentDate: '1.1.1'},
        {from: 'admin', message: 'fuck', sentDate: '1.1.1'},
        {from: 'me', message: 'ok', sentDate: '1.1.1'},
        {from: 'FuckImDead', message: 'ok', sentDate: '1.1.1'}]);
    const [message, setMessage] = useState('');
    const [isOpen, setIsOpen] = useState(false);

    const fetchChatData = async () => {
        try {
            const response = await axiosToLitres.get('/chat/history');
            setMessages([...response.data.messages.map(m => ({from: m.from, message: m.text, sentDate: m.sentDate}))])
        } catch (error) {
            toast.error(`Chat: ${error}`, {toastId: "ChatFetchError"})
        }
    };

    useEffect( () => {
        fetchChatData()

        const token = localStorage.getItem('token')
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5225/api/hubs/chat', { 
                accessTokenFactory: () => token
            })
            .withAutomaticReconnect()
            .build();

        newConnection.on('ReceiveMessage', (message) => {
            setMessages((prev) => [...prev, {from: message.from, message: message.text, sentDate: message.sentDate}]);
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

        return () => {
            if (connection) {
                connection.stop();
            }
        };

    }, []);

    const handleKeyPress = async (event) => {
        if (event.key === 'Enter')
            await sendMessage();
    }

    const sendMessage = async () => {
        if (!message) {
            toast.error("Chat: Message cannot be empty", {toastId: "ChatEmptyMessage"})
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
                    from: localStorage.getItem("username"), message: message, sentDate: new Date() }])
            }).catch((e) => toast.error(`Chat: Sending message error: ${e.message}`,
                {toastId: "ChatSendMessageError"}));

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
                    <div className="chat-messages">
                        {
                                <ChatWindow
                                    style={{maxHeight: "300px"}}
                                    messages={messages}
                                    textIfEmpty="Got a question? Write to us!">
                                </ChatWindow>
                        }
                    </div>
                    <div className="chat-input">
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
            ) : (
                <Button iconPath={ICONS.message} className="open-button" onClick={toggleChat}></Button>
            )}
        </div>
    );
};

export default Chat;
