import React from 'react';
import PropTypes from 'prop-types';
import './ChatMessage.css';
import {FileCard} from "../../FileCard/FileCard.jsx";

export function ChatMessage({ message, from, sentDate, onClick, fileModel,...rest }) {
    ChatMessage.propTypes = {
        message: PropTypes.string.isRequired,
        from: PropTypes.string.isRequired,
        sentDate: PropTypes.string.isRequired,
        onClick: PropTypes.func,
        fileModel: PropTypes.shape({
            fileId: PropTypes.string.isRequired,
            fileName: PropTypes.string.isRequired,
            fileSize: PropTypes.string.isRequired,
            createdDate: PropTypes.string.isRequired
        })
    };
    const messageClass = from === localStorage.getItem("username") ? 'chat-message own' : 'chat-message other';



    return (
        <>
            <div className={messageClass} {...rest}>
                <div className="message-info">
                    <span className="sender">{from}</span>
                    <span className="timestamp">{sentDate}</span>
                </div>
                <div className="message-text">{message}</div>
                { fileModel ? <FileCard fileName={fileModel.fileName} createdDate={fileModel.createdDate}
                                        fileSize={fileModel.fileSize} fileId={fileModel.fileId}/> : null}
            </div>
        </>
    );
}

