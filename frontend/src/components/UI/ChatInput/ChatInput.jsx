import {Input} from "../Input/Input.jsx";
import {Button} from "../Button/Button.jsx";
import React, {useRef, useState} from "react";
import "./ChatInput.css";
import {toast} from "react-toastify";
import PropTypes from "prop-types";
import {HubConnection} from "@microsoft/signalr";
import ICONS from "../../../assets/icons.jsx";
import {axiosToLitres} from "../../../hooks/useAxios.js";

//TODO: интегрировать в Chat и ChatPage
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

    //TODO: Отрабатывает только один раз почему-то
    const attachFile = (e) => {
        let f = e.target.files[0]
        if (f) {
            console.log('Файл прикреплен:', file);
            e.target.value = null;
            setFile(f);
        }
    };

    const sendFile = async () => {
        let loading = toast.loading("File is uploading...", {toastId: "FileUploadLoading"})
        const formData = new FormData();
        formData.append("file", file);
        try {
            let response = await axiosToLitres.post('file/upload', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });
            fileId = response.data;
            toast.success(`File is successfully uploaded`, {toastId: "FileUploadSuccess"})
        } catch (e) {
            toast.error(`File is not uploaded: ${e.message}`, {toastId: "FileUploadError"})
        } finally {
            toast.dismiss(loading);
        }
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
            console.log(newMessage)
            await connection
                .invoke('SendMessage', newMessage).then(() => {
                    setMessage('');
                    setMessages((prevMessages) => [...prevMessages, {
                        from: username, 
                        message: message, 
                        sentDate: new Date().toLocaleTimeString(), 
                        fileModel: fileModel 
                    }])
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
                    await sendFile();
                    await sendMessage();
                    setFile(null);
                } : sendMessage} text="Send" disabled={!message}/>
                <input
                    type="file"
                    onChange={attachFile}
                    style={{display: 'none'}}
                    ref={ref}
                />
                <Button onClick={() => ref.current.click()} className="attach-button" iconPath={ICONS.attachment}/>
            </div>
            {file ? <>
                    <p style={{maxWidth:"217px",textAlign:"center",color:"gray"}}>
                        {`File ${formatName(file.name)} ${formatSize(file.size)} attached`}</p>
                    <Button iconPath={ICONS.trash} onClick={() => setFile(null)}></Button></>
                : null}


        </div>
    )
}