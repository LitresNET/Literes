import {observable, action, makeObservable} from 'mobx';

//TODO: Добавить сохранение ролей, доступ к страницам по ролям: /account для авторизированных, /moderator для модеров и тд.
class AuthStore {
    isAuthenticated = !!localStorage.getItem('token');
    login = (token) => {
        console.log('token: ', token)
        this.isAuthenticated = true;
        localStorage.setItem('token', token);
        console.log('token from cache: ', localStorage.getItem('token'))
    }
    logout = () => {
        console.log('logout performed')
        this.isAuthenticated = false;
        localStorage.removeItem('token');
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