import { RouterProvider } from "react-router-dom";
import router from './router/router';
import {ToastContainer} from "react-toastify";
import Chat from "./layouts/Chat/Chat.jsx";
import authStore from "./store/store.js";

function App() {
    const shouldHideChat = window.location.pathname.endsWith('/chat');
    return (
        <>
            <RouterProvider router={router}></RouterProvider>
            <ToastContainer />
            {authStore.isAuthenticated && !shouldHideChat ? <Chat /> : null}
        </>
    );
}
export default App;
