import './FileCard.css'
import PropTypes from "prop-types";
import {axiosToLitres} from "../../hooks/useAxios.js";
import configData from "./../../../config.json";

export function FileCard({ fileId, fileName, fileSize, createdDate, ...rest}) {
    FileCard.propTypes = {
        fileId: PropTypes.string.isRequired,
        fileName: PropTypes.string.isRequired,
        fileSize: PropTypes.string.isRequired,
        createdDate: PropTypes.string.isRequired
    }
    // const getFile = async () => {
    //     return await axiosToLitres.get()
    // }
    const formatSize = (size) => {
        if (size === 0) return '0 Bytes';
        const k = 1024; // 1 КБ = 1024 Б
        const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB']; // Масштабирование
        const i = Math.floor(Math.log(size) / Math.log(k)); // Находим индекс
        return parseFloat((size / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i]; // Возвращаем отформатированный размер
    };

    return (
        <div className="file-card" {...rest}>
            <div className="file-name">{<a target="_blank" href={`${configData.LITRES_URL}` + `/file/${fileId}`}>{fileName}</a>}</div>
            <div className="file-info">{`${formatSize(fileSize)} | ${createdDate}`}</div>
        </div>
    )

}