import {axiosToLitres} from "./useAxios.js";

export async function useGetBookByCategory(pageNumber, amount, props) {
    const { SearchedValue, Category, New, HighRated } = props // string, string, bool, bool

    try {
        let url = `/book/catalog/${pageNumber}/${amount}?${SearchedValue === null || SearchedValue === undefined ? '' : 'Name=' + SearchedValue + "&"}${Category === null || Category === undefined ? '' : 'category=' + Category + "&"}${New === null || New === undefined ? '' : 'new=' + New + "&"}${HighRated === null || HighRated === undefined ? '' : 'highrated=' + HighRated}`;

        const response = await axiosToLitres.get(url);
        if (response.status === 200) {
            return { result: response.data, error: null };
        } else {
            return { result: null, error: response.data };
        }
    } catch (e) {
        //TODO: убрать логирование в консоль в prod версии
        console.error(e);
        return { result: null, error: 'Опаньки... Что-то пошло не так'};
    }
}
