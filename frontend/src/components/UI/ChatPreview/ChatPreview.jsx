import "./ChatPreview.css";
import PropTypes from "prop-types";
import React from "react";

//TODO: добавить функционал прочитанного/непрочитанного
export function ChatPreview({userId, username, lastMessageDate, ...rest }){
    ChatPreview.propTypes = {
        userId: PropTypes.number.isRequired,
        username: PropTypes.string,
        lastMessageDate: PropTypes.string.isRequired
    }
    return(
        <>
            <div className="chat-preview" {...rest}>
                <div className="chat-sender">
                    <span className="username">{username}</span>
                    <span className="userid">id: {userId}</span>
                </div>
                <div className="chat-timestamp">
                <span className="chat-timestamp">{!lastMessageDate || lastMessageDate === "00:00:00" ? null : lastMessageDate }</span>
                </div>
            </div>
        </>
    )
}