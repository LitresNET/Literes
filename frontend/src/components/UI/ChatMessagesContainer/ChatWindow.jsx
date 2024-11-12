import PropTypes from "prop-types";
import "./ChatWindow.css";

export function ChatWindow({width, height, maxHeight, messages, ...rest}) {
    ChatWindow.propTypes = {
        maxHeight: PropTypes.string,
    }
    return (
        <div className="chat-window" style={{maxHeight: maxHeight ?? "300px"}}>

        </div>
    )
}