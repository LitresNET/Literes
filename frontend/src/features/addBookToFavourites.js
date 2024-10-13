import {axiosToLitres} from "../hooks/useAxios.js";
import {toast} from "react-toastify";

//TODO: Добавить отслеживание состояния уже добавленных в избранное книг (чтоб значок избранного подсвечивался)
// После добавления состояния разделить функцию на два запроса: добавление и удаление (сейчас на бэке временно
// универсальный метод)
export const addBookToFavourites = async (bookId) => {
    return await axiosToLitres.post(`user/favourite/${bookId}`)
        .then(() => toast.success("The book has been successfully added to your favorites!",
            {toastId: "AddBookToFavouritesSuccess"}))
        .catch((e) => toast.error("Add Book To Favourites: " + e.message,
            {toastId: "AddBookToFavouritesError"}));
}