import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import './Chat.css'; // Добавьте стили для вашего чата
import ICONS from "../../assets/icons.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";
import {Input} from "../../components/UI/Input/Input.jsx";
import {toast} from "react-toastify";

//TODO: добавить toast уведомление при новом сообщении
const Chat = () => {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([
        // test messages
        {user: 'admin', message: 'fuck'},
        {user: 'admin', message: 'fuck'},
        {user: 'admin', message: 'fucksdakdsl;adksa;ldksa;dksadkakdassdaskdjasdjaskdjajkd'},
        {user: 'admin', message: 'fuck'},
        {user: 'me', message: 'ok'},
        {user: 'me', message: 'ok'}
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
                setMessages((prevMessages) => [...prevMessages, { user, message }])
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
        //TODO: закомментирован псевдофункционал для теста фронта
        if (// connection &&
             message) {
          //  await connection.invoke('SendMessage', 'user', message);
            setMessage('');
            setMessages((prevMessages) => [...prevMessages, { user: "me", message }])
        }
        else {
            toast.error("chat don't working")
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
                            messages.length ?
                            messages.map((msg, index) => (
                            <div className={"message"} key={index}>
                                <strong>{msg.user}: </strong>
                                <p>{msg.message}</p>
                            </div>
                        )) : <p style={{textAlign: "center", fontWeight: 'bold'}}>Got a question? Write to us!</p>}
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
