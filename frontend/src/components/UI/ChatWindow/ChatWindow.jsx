import PropTypes from "prop-types";
import "./ChatWindow.css";
import React, {useEffect, useRef} from "react";
import {ChatMessage} from "../ChatMessage/ChatMessage.jsx";

export function ChatWindow({messages, textIfEmpty, isOpen, ...rest}) {
    ChatWindow.propTypes = {
        messages: PropTypes.arrayOf(
            PropTypes.shape({
                from: PropTypes.string.isRequired,
                message: PropTypes.string.isRequired,
                sentDate: PropTypes.string.isRequired,
                fileModel: PropTypes.shape({
                    fileId: PropTypes.string.isRequired,
                    fileName: PropTypes.string.isRequired,
                    fileSize: PropTypes.number.isRequired,
                    createdDate: PropTypes.string.isRequired
            })})),
        textIfEmpty: PropTypes.string,
        isOpen: PropTypes.bool
    }
    const chatWindowRef = useRef(null);
    const makeScrollDown = () =>
    {
        if (chatWindowRef.current)
            chatWindowRef.current.scrollTop = chatWindowRef.current.scrollHeight;
    }
    useEffect(() => {
            makeScrollDown();
    }, [messages, isOpen]);

    return (
        <div ref={chatWindowRef} className="chat-window" {...rest}>
            {
                messages?.length ?
                messages.map((msg, index) =>
                    <ChatMessage key={index} message={msg.message} from={msg.from} sentDate={msg.sentDate} fileModel={msg.fileModel}></ChatMessage>) :
                    <p style={{textAlign: "center", fontWeight: 'bold'}}> {textIfEmpty ?? "Chat is empty"}</p>
            }

        </div>
    );
}