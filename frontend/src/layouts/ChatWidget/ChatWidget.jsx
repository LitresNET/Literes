import React, { useState } from 'react';
import './ChatWidget.css';
import ICONS from "../../assets/icons.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";
import {toast} from "react-toastify";
import {Chat} from "../../components/Chat/Chat.jsx";
import {useNavigate} from "react-router-dom";

const ChatWidget = () => {
    const [isOpen, setIsOpen] = useState(false);
    const navigate = useNavigate();
    const toggleChat = () => {
        setIsOpen(!isOpen);
        toast.dismiss({id:"ChatNewMessage"});
    };

    return (
            <div className={"chat-container" + (isOpen ? " open" : "")}>
                <div style={{ display: isOpen ? 'none' : 'block' }}>
                    <Button
                        iconPath={ICONS.message}
                        className={"open-button"}
                        onClick={localStorage.getItem("roleName") === "Agent" ? () => navigate('/chat') : toggleChat}
                    />
                </div>
                {localStorage.getItem("roleName") === "Agent" ? null :
                <div style={{ display: isOpen ? 'block' : 'none' }}>
                    <Button   iconPath={ICONS.caret_down}
                              className={"close-button"}
                              onClick={toggleChat}/>
                    <Chat chatWindowStyle={{ maxHeight: "300px" }} isOpen={isOpen} textIfEmpty="Got a question? Write to us!" />
                </div> }
            </div>
    );
};

export default ChatWidget;
