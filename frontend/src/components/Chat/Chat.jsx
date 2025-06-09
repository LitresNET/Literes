import { ChatWindow } from "../UI/ChatWindow/ChatWindow.jsx";
import React, { useEffect, useState, useRef } from "react";
import { ChatInput } from "../UI/ChatInput/ChatInput.jsx";
import { toast } from "react-toastify";
import { axiosToLitres } from "../../hooks/useAxios.js";
import PropTypes from "prop-types";
import { GrpcWebFetchTransport } from '@protobuf-ts/grpcweb-transport';
import { ChatServiceClient } from './../../grpc/chat/chat.client';
import { Empty } from 'google-protobuf/google/protobuf/empty_pb';

import './Chat.css';

export function Chat({ chatWindowStyle, chatInputStyle, textIfEmpty, isOpen, ...rest }) {
  Chat.propTypes = {
    textIfEmpty: PropTypes.string,
  };

  const [messages, setMessages] = useState([]);
  const [chatId, setChatId] = useState(null);
  const clientRef = useRef(null);
  const abortRef  = useRef(null);

  const fetchChatData = async () => {
    try {
      const response = await axiosToLitres.get('/chat/history');
      setMessages(response.data.messages?.map(m => ({
        chatId: m.chatId,
        from: m.from,
        message: m.text,
        sentDate: new Date(m.sentDate).toLocaleTimeString(),
        fileModel: m.fileModel,
      })) || []);
    } catch (error) {
      toast.error(`Chat: ${error}`, { toastId: "ChatFetchError" });
    }
  };

  useEffect(() => {
    // инициализация клиента и подписки
    if (clientRef.current) return;

    const token = localStorage.getItem('token');
    const meta  = { authorization: `Bearer ${token}` };

    abortRef.current = new AbortController();
    const transport = new GrpcWebFetchTransport({
      baseUrl: 'https://localhost:5225',
      signal: abortRef.current.signal,
    });
    const client = new ChatServiceClient(transport);
    clientRef.current = client;

    // подписываемся на сообщения
    (async () => {
      try {
        const stream = client.subscribe(new Empty(), {meta});
        for await (const resp of stream.responses) {
          if (chatId === null) {
            setChatId(resp.chatId);
          }
          const ms = Number(resp.sentUnix);    // convert bigint → number
          const time = new Date(ms).toLocaleTimeString();
          setMessages(prev => [
            ...prev,
            {
              chatId: resp.chatId,
              from: resp.from,
              message: resp.text,
              sentDate: time,
              fileModel: resp.file,
            }
          ]);
          toast.info('Chat: New Message', { toastId: 'ChatNewMessage', autoClose: false });
        }
      } catch (err) {
        if (err.name !== 'AbortError') {
          toast.error(`Chat: Stream error (${err.message})`, { toastId: 'ChatStreamError' });
        }
      }
    })();

    fetchChatData();
    return () => {
      abortRef.current.abort();
    };
  }, [chatId]);

  const sendMessage = async ({ newMsg }) => {
    const client = clientRef.current;
    const token = localStorage.getItem('token');
    const meta  = { authorization: `Bearer ${token}` };
    console.log('got: ', newMsg)
    if (!client || chatId === null) {
      toast.error('Chat: not connected', { toastId: 'ChatNotConnected' });
      return;
    }

    newMsg.chatId = chatId;
    try {
      await client.sendMessage(newMsg, {meta});
    } catch (err) {
      toast.error(`Chat: failed to send (${err.message})`, { toastId: 'ChatSendError' });
    }
  };

  return (
    <div className="chat" {...rest}>
      <ChatWindow
        messages={messages}
        textIfEmpty={textIfEmpty}
        isOpen={isOpen}
        style={chatWindowStyle}
      />
      <ChatInput
        sendMessage={sendMessage}
        setMessages={setMessages}
        style={chatInputStyle}
      />
    </div>
  );
}