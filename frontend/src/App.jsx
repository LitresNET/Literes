import { RouterProvider } from "react-router-dom";
import router from './router/router';
import {ToastContainer} from "react-toastify";
import ChatWidget from "./layouts/ChatWidget/ChatWidget.jsx";
import authStore from "./store/store.js";

function App() {
    return (
        <>
            <RouterProvider router={router}></RouterProvider>
            <ToastContainer />
            {authStore.isAuthenticated &&
            localStorage.getItem("roleName") !== "Agent"? <ChatWidget /> : null}
        </>
    );
}
export default App;
