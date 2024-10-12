import ICONS from "../../../assets/icons.jsx";
import COLORS from '../../../assets/colors.jsx';
import {Button} from "../../../components/UI/Button/Button.jsx";
import {Input} from "../../../components/UI/Input/Input.jsx";
import {Banner} from "../../../components/UI/Banner/Banner.jsx";

import { Link, useNavigate } from 'react-router-dom';
import useAuth from '../../../hooks/useAuth.js';
import { useState } from 'react';
import './SignInPage.css';
import GoogleButton from "react-google-button";
import configData from "../../../../config.json";

const SignInPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [validationError, setError] = useState('');
    const { loading, login } = useAuth();
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        //TODO: Добавить больше валидации
        if (!email || !password) {
            setError('Please fill in all fields');
            return;
        } else { setError('') }

        const userData = {
            email: email,
            password: password,
        };

        await login(userData).then((result) => {
            if (result.error) {
                setError(result.error);
            } else {
                navigate("/home")
            }
        });
    };

    return (
        <>
            <div className={'container-sign-in'}>
                <div className={'title-sign-in'}>
                    <h1>SIGN IN</h1>
                </div>
                <Banner>
                    <form className={'sign-in-form'}>
                        <div className={'label-input-sign-in'}>
                            <label htmlFor={'email'}>Enter your email</label>
                            <Input
                                className="input-sign-in"
                                id="email"
                                placeholder="example@example.com"
                                type="text"
                                onChange={(e) => setEmail(e.target.value)}/>
                        </div>
                        <div className={'label-input-sign-in'}>
                            <label htmlFor={'password'}>Enter your password</label>
                            <Input
                                className="input-sign-in"
                                id="password"
                                placeholder="********"
                                type="password"
                                onChange={(e) => setPassword(e.target.value)}
                            />
                        </div>
                        <a className={'forgot-sign-in'}>Forgot password?</a>
                        <div className={'button-sign-in'}>
                            <Button
                                text={loading ? "Wait..." : "Sign in"}
                                onClick={handleLogin}
                                round={"true"}
                                color={"yellow"}
                                iconpath={ICONS.sign_in}
                            />
                        </div>
                        {validationError &&
                            <p className="sign-up-error">{validationError}</p>
                        }
                    </form>
                </Banner>
                <div className={'dont-sign-in'}>
                    <GoogleButton
                        onClick={() => window.location.href = (`${configData.LITRES_URL}/signin/google`)}/>
                    <p>Don’t registered yet?</p>
                    <p><Link to="/signup" style={{color: COLORS.blue}}>Create an account</Link> in a few steps -{'>'}</p>
                </div>
            </div>
        </>
    )
}

export default SignInPage