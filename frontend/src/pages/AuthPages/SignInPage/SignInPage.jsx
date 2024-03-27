import './SignInPage.css';
import {Button} from "../../../components/UI/Button/Button.jsx";
import {Input} from "../../../components/UI/Input/Input.jsx";
import ICONS from "../../../assets/icons.jsx";

const SignInPage = () => {

    return (
        <>
            <div className={'container-sign-in'}>
                <div className={'title-sign-in'}>
                    <h1>SIGN IN</h1>
                </div>
                <div className={'form-container-sign-in'}>
                    <form className={'sign-in-form'}>
                        <div className={'label-input-sign-in'}>
                            <label htmlFor={'email'}>Enter your email</label>
                            <Input class="input-sign-in" id="email" placeholder="example@example.com" type="text"/>
                        </div>
                        <div className={'label-input-sign-in'}>
                            <label htmlFor={'password'}>Enter your password</label>
                            <Input class="input-sign-in" id="password" placeholder="********" type="text"/>
                        </div>
                        <a className={'forgot-sign-in'}>Forgot password?</a>
                        <div className={'button-sign-in'}>
                        <Button text={"Sign in"} onClick={() => alert("TODO")} round={"true"} color={"yellow"}
                                iconpath={ICONS.sign_in}/>
                        </div>
                    </form>
                </div>
                <div className={'dont-sign-in'}>
                    <a>Don’t registered yet? Create an account in a few steps -{'>'}</a>
                </div>
            </div>
        </>
    )
}

export default SignInPage