import {axiosToLitres} from "./useAxios.js";

export async function useGetBookByCategory(pageNumber, amount, props) {
    const { Category, New, HighRated} = props // string,bool,bool

    try {
        const response = await axiosToLitres.get(`/book/catalog/${pageNumber}/${amount}?
        ${Category === null || Category === undefined ? '' : 'category='+Category} 
        ${New === null || New === undefined ? '' : 'new='+New}
        ${HighRated === null || HighRated === undefined ? '' : 'highrated='+New}`);
        if (response.status === 200) {
            return { result: response.data, error: null };
        } else {
            return { result: null, error: response.data };
        }
    } catch (e) {
        return { result: null, error: e.response.data.errors[0].description };
    }
}
