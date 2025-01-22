import {observable, action, makeObservable} from 'mobx';
import {toast} from "react-toastify";
import {axiosToLitres} from "../hooks/useAxios.js";

//TODO: Добавить сохранение ролей, доступ к страницам по ролям: /account для авторизированных, /moderator для модеров и тд.
//TODO: Сохранять в localStorage не отдельные поля, а объект юзера с нужными полями (токен, роль, имя и тд)
class AuthStore {
    isAuthenticated = !!localStorage.getItem('token');
    login = async (token) => {
        toast.info("You have successfully logged in, " /* + localStorage.getItem("Username") */,
            {toastId: "LogInInfo"})
        this.isAuthenticated = true;
        localStorage.setItem('token', token);
        const userData = await axiosToLitres.get(`/user/settings`);
        localStorage.setItem("user", userData.data);
        localStorage.setItem("userId", userData.data.id);
        localStorage.setItem("username", userData?.data.name);
        localStorage.setItem("roleName", userData?.data.roleName)
        location.reload();
    }
    logout = () => {
        toast.info("You have successfully logged out", {toastId: "LogOutInfo"})
        this.isAuthenticated = false;
        //TODO: вторая TODO сверху именно для исправления этой ситуации
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        localStorage.removeItem('userId')
        localStorage.removeItem('username');
        localStorage.removeItem('roleName')
        location.reload();
    }
    constructor() { makeObservable(this, {
        isAuthenticated: observable,
        login: action,
        logout: action
        })
    }
}

const authStore = new AuthStore();

export default authStore;