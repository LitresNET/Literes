import "./ChatPreview.css";
import PropTypes from "prop-types";
import React from "react";

//TODO: добавить функционал прочитанного/непрочитанного
export function ChatPreview({userId, username, lastMessageDate, ...rest }){
    ChatPreview.propTypes = {
        userId: PropTypes.string.isRequired,
        username: PropTypes.string.isRequired,
        lastMessageDate: PropTypes.instanceOf(Date).isRequired,
    }
    return(
        <>
            <div className="chat-preview" {...rest}>
                <div className="chat-sender">
                    <span className="username">{username}</span>
                    <span className="userid">id: {userId}</span>
                </div>
                <div className="chat-timestamp">
                <span className="chat-timestamp">{new Date(lastMessageDate).toLocaleTimeString()}</span>
                </div>
            </div>
        </>
    )
}