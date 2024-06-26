import axios from 'axios';
import configData from "./../../config.json";

export const axiosToLitres = axios.create({
    baseURL: `${configData.LITRES_URL}`,
    timeout: 30000,
    headers: {
        'Authorization': 'Bearer ' + localStorage.token
    }
});

export const axiosToPayment = axios.create({
    baseURL: `${configData.PAYMENT_URL}`,
    timeout: 30000
})