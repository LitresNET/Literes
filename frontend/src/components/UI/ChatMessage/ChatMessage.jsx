import React from 'react';
import PropTypes from 'prop-types';
import './ChatMessage.css';

export function ChatMessage({ message, from, sentDate, onClick, ...rest }) {
    ChatMessage.propTypes = {
        message: PropTypes.string.isRequired,
        from: PropTypes.string.isRequired,
        sentDate: PropTypes.string.isRequired,
        onClick: PropTypes.func
    };
    const messageClass = from === localStorage.getItem("username") ? 'chat-message own' : 'chat-message other';



    return (
        <>
            <div className={messageClass} {...rest}>
                <div className="message-info">
                    <span className="sender">{from}</span>
                    <span className="timestamp">{new Date(sentDate).toLocaleTimeString()}</span>
                </div>
                <div className="message-text">{message}</div>
            </div>
        </>
    );
}

