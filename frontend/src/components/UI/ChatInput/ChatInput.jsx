import {Input} from "../Input/Input.jsx";
import {Button} from "../Button/Button.jsx";
import React, {useRef, useState} from "react";
import "./ChatInput.css";
import {toast} from "react-toastify";
import PropTypes from "prop-types";
import {HubConnection} from "@microsoft/signalr";
import ICONS from "../../../assets/icons.jsx";
import {axiosToLitres} from "../../../hooks/useAxios.js";

export function ChatInput({connection, setMessages, ...rest}) {
    ChatInput.propTypes = {
        connection: PropTypes.shape(HubConnection),
        setMessages: PropTypes.func
    }
    const [message, setMessage] = useState('');
    const username = localStorage.getItem("username");
    const [file, setFile] = useState(null);
    let fileId;
    const ref = useRef();

    const attachFile = (e) => {
        let f = e.target.files[0]
        if (f) {
            console.log('Файл прикреплен:', f);
            e.target.value = null;
            setFile(f);
        }
    };

    const sendFile = async () => {
        let isSuccess = false;
        let loading = toast.loading("File is uploading...", {toastId: "FileUploadLoading"})
        const formData = new FormData();
        formData.append("file", file);
        await axiosToLitres.post('file/upload', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        }).then((response) => {
            fileId = response.data;
            toast.success(`File is successfully uploaded`, {toastId: "FileUploadSuccess"});
            isSuccess = true;
        }).catch((e) => {
            console.log(e)
            toast.error(`File is not uploaded: ${e.message}`, {toastId: "FileUploadError"});
        }).finally(() => toast.dismiss(loading));
        return isSuccess
    }
    const handleKeyPress = async (event) => {
        if (event.key === 'Enter')
            await sendMessage();
    }
    const sendMessage = async () => {
        if (!message) {
            toast.error("Chat: Message cannot be empty", {toastId: "ChatEmptyMessage"})
            return
        }
        if (connection) {
            let fileModel = null;
            if (file) {

                fileModel = {
                    fileId: fileId,
                    fileName: file?.name,
                    fileSize: file?.size,
                    createdDate: new Date().toLocaleTimeString()
                }
            }
            const newMessage = {
                Text: message,
                From: username,
                FileModel: fileModel
            };
            await connection
                .invoke('SendMessage', newMessage).then(() => {
                    setMessage('');
                    setMessages((prevMessages) => [...prevMessages, {
                        from: username, 
                        message: message, 
                        sentDate: new Date().toLocaleTimeString(), 
                        fileModel: fileModel 
                    }]) // TODO: Если серверу не удаётся загрузить файл, он все равно отправляет сообщение, но без него, а файл визуально все равно отображается.
                }).catch((e) => toast.error(`Chat: Sending message error: ${e.message}`,
                    {toastId: "ChatSendMessageError"}));
        }
        else {
            toast.error("Chat: No connection", {toastId: "ChatSendMessageError"});
        }
    };
    const formatName = (name) => {
        if (name.length > 10) {
            return `${name.substring(0, 10)}...`; // Обрезаем имя и добавляем многоточие
        }
        return name;
    };
    const formatSize = (size) => {
        if (size === 0) return '0 Bytes';
        const k = 1024; // 1 КБ = 1024 Б
        const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB']; // Масштабирование
        const i = Math.floor(Math.log(size) / Math.log(k)); // Находим индекс
        return parseFloat((size / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i]; // Возвращаем отформатированный размер
    };

    return (
        <div className="chat-input" {...rest}>
            <Input
                type="text"
                value={message}
                onChange={(e) => setMessage(e.target.value)}
                placeholder="Write message"
                onKeyDown={handleKeyPress}
            />
            <div className="chat-input-buttons">
                <Button onClick={file ? async () => {
                    if (await sendFile()) {
                        await sendMessage();
                        setFile(null);
                    }

                } : sendMessage} text="Send" disabled={!message}/>
                <input
                    type="file"
                    onChange={attachFile}
                    style={{display: 'none'}}
                    ref={ref}
                />
                <Button onClick={() => ref.current.click()} className="small-button" iconPath={ICONS.attachment}
                        title="Attach file"/>
            </div>
            {file ? <>
                    <p style={{maxWidth:"217px",textAlign:"center",color:"gray"}}>
                        {`File ${formatName(file.name)} ${formatSize(file.size)} attached`}</p>
                    <div className="chat-input-buttons">
                        <Button iconPath={ICONS.trash} onClick={() => setFile(null)} title={"Remove file"}/>
                        <Button iconPath={ICONS.pencil} className="small-button" title={"Add metadata"}/>
                    </div>
                    </>
                : null}


        </div>
    )
}