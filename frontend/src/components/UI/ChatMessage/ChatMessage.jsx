import React from 'react';
import PropTypes from 'prop-types';
import './ChatMessage.css'; // Подключаем CSS для стилей

export function ChatMessage({ text, sender, timestamp, ...rest }) {
    ChatMessage.propTypes = {
        text: PropTypes.string.isRequired,
        sender: PropTypes.string.isRequired,
        timestamp: PropTypes.instanceOf(Date).isRequired     //TODO: переименовать на sentDate
    };
    const messageClass = sender === localStorage.getItem("username") ? 'chat-message own' : 'chat-message other';



    return (
        <>
                <div className={messageClass} {...rest}>
                    <div className="message-info">
                        <span className="sender">{sender}</span>
                        <span className="timestamp">{new Date(timestamp).toLocaleTimeString()}</span>
                    </div>
                    <div className="message-text">{text}</div>
                </div>

        </>
    );
}

