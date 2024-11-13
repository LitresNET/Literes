import {observable, action, makeObservable} from 'mobx';
import {toast} from "react-toastify";

//TODO: Добавить сохранение ролей, доступ к страницам по ролям: /account для авторизированных, /moderator для модеров и тд.
class AuthStore {
    isAuthenticated = !!localStorage.getItem('token');
    login = (token) => {
        //TODO: было бы здорово после входа получать от бэка не только токен, но и имя пользователя.
        // Сейчас оно сохраняется в localstorage при автопереходе на /account, поэтому сразу после входа недоступно
        toast.info("You have successfully logged in, " /* + localStorage.getItem("Username") */,
            {toastId: "LogInInfo"})
        this.isAuthenticated = true;
        localStorage.setItem('token', token);
    }
    logout = () => {
        toast.info("You have successfully logged out", {toastId: "LogOutInfo"})
        this.isAuthenticated = false;
        localStorage.removeItem('token');
        localStorage.removeItem('username');
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