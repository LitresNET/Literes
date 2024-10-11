import {observable, action, makeObservable} from 'mobx';

class AuthStore {
    isAuthenticated = !!localStorage.getItem('token');
    login = (token) => {
        this.isAuthenticated = true;
        localStorage.setItem('token', token);
    }
    logout = () => {
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