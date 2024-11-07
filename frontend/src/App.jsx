import { RouterProvider } from "react-router-dom";
import router from './router/router';
import {ToastContainer} from "react-toastify";
import Chat from "./layouts/Chat/Chat.jsx";

function App() {
    return (
        <>
            <RouterProvider router={router}></RouterProvider>
            <ToastContainer />
            <Chat />
        </>
    );
}
export default App;
