import PropTypes from "prop-types";
import "./ChatWindow.css";
import React, {useEffect, useRef} from "react";
import {ChatMessage} from "../ChatMessage/ChatMessage.jsx";

export function ChatWindow({messages, textIfEmpty, ...rest}) {
    ChatWindow.propTypes = {
        messages: PropTypes.arrayOf(
            PropTypes.shape({
                from: PropTypes.string.isRequired,
                message: PropTypes.string.isRequired,
                sentDate: PropTypes.string.isRequired
            })),
        textIfEmpty: PropTypes.string
    }
    const chatWindowRef = useRef(null);
    const makeScrollDown = () =>
    {
        if (chatWindowRef.current)
            chatWindowRef.current.scrollTop = chatWindowRef.current.scrollHeight;
    }
    useEffect(() => {
            makeScrollDown();
    }, [messages]);

    return (
        <div ref={chatWindowRef} className="chat-window" {...rest}>
            {
                messages?.length ?
                messages.map((msg, index) =>
                    <ChatMessage key={index} message={msg.message} from={msg.from} sentDate={msg.sentDate}></ChatMessage>) :
                    <p style={{textAlign: "center", fontWeight: 'bold'}}> {textIfEmpty ?? "Chat is empty"}</p>
            }

        </div>
    );
}