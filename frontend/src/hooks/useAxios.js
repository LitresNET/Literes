import axios from 'axios';
import configData from "./../../config.json";

async function fetchData(url){
    //TODO: Подумать над использованием fetchData
}
export const axiosToLitres = axios.create({
    baseURL: `${configData.LITRES_URL}`,
    withCredentials: true,
    timeout: 30000
});

axiosToLitres.interceptors.request.use((config) => {
    config.headers.Authorization = `Bearer ${localStorage.getItem('token')}`;
    return config;
})

export const axiosToPayment = axios.create({
    baseURL: `${configData.PAYMENT_URL}`,
    timeout: 30000
})