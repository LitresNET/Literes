import './FileCard.css'
import PropTypes from "prop-types";

export function FileCard({ fileName }) {
    FileCard.propTypes = {
        fileName: PropTypes.string.isRequired,
        fileSize: PropTypes.string.isRequired,

    }
}