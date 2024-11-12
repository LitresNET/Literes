import "./ChatPage.css";
import {ChatWindow} from "../../../components/UI/ChatMessagesContainer/ChatWindow.jsx";

//TODO: доделать
export default function ChatPage(){
    return (

        <div className="chat-page">

            <div className="chat-preview">
                //TODO: заменить на chatPreview
                <div className="chat-preview-item">Чат 1</div>
                <div className="chat-preview-item">Чат 2</div>
                <div className="chat-preview-item">Чат 3</div>
            </div>
            <div className="chat-window">
                <ChatWindow/>
            </div>
        </div>
    );
}