import {Input} from "../Input/Input.jsx";
import {Button} from "../Button/Button.jsx";
import React, {useState} from "react";
import "ChatInput.css";
import {toast} from "react-toastify";

//TODO: интегрировать в Chat и ChatPage
export function ChatInput() {
    const [message, setMessage] = useState('');
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
    return (
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
    )
}