import {Input} from "../../../components/UI/Input/Input.jsx";
import {Button} from "../../../components/UI/Button/Button.jsx";
import ICONS from "../../../assets/icons.jsx";
import "./SignUpPage.css";
import {Banner} from "../../../components/UI/Banner/Banner.jsx";

const SignUpPage = () => {

    return (
        <>
            <div className={'container-sign-up'}>
                <div className={'title-sign-up'}>
                    <h1>SIGN UP</h1>
                </div>
                <Banner>
                    <form className={'sign-up-form'}>
                        <div className={'label-input-sign-up'}>
                            <label htmlFor={'name'}>Enter your name</label>
                            <Input class="input-sign-up" id="name" placeholder="name" type="text"/>
                        </div>
                        <div className={'label-input-sign-up'}>
                            <label htmlFor={'email'}>Enter your email</label>
                            <Input class="input-sign-up" id="email" placeholder="example@example.com" type="text"/>
                        </div>
                        <div className={'label-input-sign-up'}>
                            <label htmlFor={'password'}>Type your password</label>
                            <Input class="input-sign-up" id="password" placeholder="********" type="text"/>
                        </div>
                        <div className={'label-input-sign-up'}>
                            <label htmlFor={'retype-password'}>Retype your password</label>
                            <Input class="input-sign-up" id="retype-password" placeholder="********" type="text"/>
                        </div>
                        <div className={'button-sign-up'}>
                            <Button text={"Continue"} onClick={() => alert("TODO")} round={"true"} color={"yellow"}
                                    iconpath={ICONS.next}/>
                        </div>
                    </form>
                </Banner>
                <div className={'have-sign-up'}>
                    <a><p>Already have an account?</p> <p>Sign in -{'>'}</p></a>
                </div>
            </div>
        </>
    )
}

export default SignUpPage