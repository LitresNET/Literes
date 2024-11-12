import "./ChatPreview.css";
import PropTypes from "prop-types";

export function ChatPreview({userId, username, lastMessageDate }){
    ChatPreview.propTypes = {
        userId: PropTypes.string.isRequired,
        username: PropTypes.string.isRequired,
        lastMessageDate: PropTypes.instanceOf(Date).isRequired
    }
    // TODO: ChatPreview сделать
    return(
        <></>
    )
}