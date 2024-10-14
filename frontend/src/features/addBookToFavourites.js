import {axiosToLitres} from "../hooks/useAxios.js";

export const addBookToFavourites = async (bookId) => {
    return await axiosToLitres.post(`user/favourite/${bookId}`);
}