import { useState } from "react";
import { axiosToLitres } from "./useAxios.js";
import authStore from "../store/store.js";

const useAuth = () => {
  const [loading, setLoading] = useState(false);

  async function register(userData) {
    setLoading(true);
    return await axiosToLitres.post(`/signup/user`, userData)
        .then((response) => {
        if (response.status === 200) {
          authStore.login(response.data);
          return {error: null };
        } else {
          return {error: response.data };
        }}).catch((e) => {
        //Ошибка (не 2хх код) в ответе от бэка
        if (e.response) {
          //.Message - ошибки кастомного Middleware, остальные - ошибки валидатора в контроллере
          return { error: e.response.data.Message ? e.response.data.Message : e.response.data.errors.Password ?
                e.response.data.errors.Password[0] : e.response.data.errors.Email ? e.response.data.errors.Email[0] :
                e.response.data.errors.Name[0]};
        }
        //Остальные ошибки (не пришел ответ, не отправился запрос и тд)
        else {
          //TODO: убрать логирование в консоль в prod версии
          console.error(e);
          return {error: 'Опаньки... Что-то пошло не так'};
        }}).finally(() => setLoading(false));
      }

  async function login(userData) {
    setLoading(true);
    return await axiosToLitres.post(`/signin`, userData)
        .then((response) => {
          if (response.status === 200) {
            authStore.login(response.data);
            return {error: null };
          } else {
            return {error: response.data };
          }})
        .catch((e) => {
        //Ошибка (не 2хх код) в ответе от бэка
        if (e.response) {
          //.Message - ошибки кастомного Middleware, остальные - ошибки валидатора в контроллере
          return { error: e.response.data.Message ? e.response.data.Message : e.response.data.errors.Password ?
                e.response.data.errors.Password[0] : e.response.data.errors.Email[0]} ;
        }
        //Остальные ошибки (не пришел ответ, не отправился запрос и тд)
        else {
          //TODO: убрать логирование в консоль в prod версии
          console.error(e);
          return {error: 'Опаньки... Что-то пошло не так'};
        }}).finally(() => setLoading(false));
  }

  return { loading, register, login };
};

  export default useAuth;