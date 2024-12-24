import {Input} from "../Input/Input.jsx";
import {Button} from "../Button/Button.jsx";
import React, {useState} from "react";
import "./ChatInput.css";
import {toast} from "react-toastify";
import PropTypes from "prop-types";
import {HubConnection} from "@microsoft/signalr";

//TODO: интегрировать в Chat и ChatPage
export function ChatInput({connection, setMessages, ...rest}) {
    ChatInput.propTypes = {
        connection: PropTypes.shape(HubConnection),
        setMessages: PropTypes.func
    }
    const [message, setMessage] = useState('');
    const username = localStorage.getItem("username");
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
                From: username
            };
            await connection.invoke('SendMessage', newMessage).then(() => {
                setMessage('');
                setMessages((prevMessages) => [...prevMessages, {
                    from: username, message: message, sentDate: new Date().toLocaleTimeString() }])
            }).catch((e) => toast.error(`Chat: Sending message error: ${e.message}`,
                {toastId: "ChatSendMessageError"}));

        }
        else {
            toast.error("Chat: No connection", {toastId: "ChatSendMessageError"});
        }
    };
    return (
        <div className="chat-input" {...rest}>
            <Input
                type="text"
                value={message}
                onChange={(e) => setMessage(e.target.value)}
                placeholder="Write message"
                onKeyDown={handleKeyPress}
            />
            <Button onClick={sendMessage} text="Send" disabled={!message}></Button>
        </div>
    )
}