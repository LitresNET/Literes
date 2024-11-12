import './UserSettingsPage.css'
import ICONS from '../../../assets/icons'
import IMAGES from '../../../assets/images'
import { Cover } from '../../../components/Cover/Cover'
import { Input } from '../../../components/UI/Input/Input'
import { Button } from '../../../components/UI/Button/Button'
import {useNavigate} from 'react-router-dom'
import {useState} from "react";
import {axiosToLitres} from "../../../hooks/useAxios.js";

export default function UserSettingsPage() {
    const [email, setEmail] = useState('');
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [validationError, setError] = useState(false);
    const navigate = useNavigate();

    const handleChange = async (e) => {
        e.preventDefault();

        let response;
        const changeData = {
            email: email,
            oldPassword: oldPassword,
            newPassword: newPassword
        };

        if (oldPassword !== newPassword) {
            setError(true);
        } else {
            setError(false)

            try {
                await axiosToLitres.post(`user/settings`, changeData);
                navigate('/account');
            } catch (e) {
                return {result: null, error: e.response.data.errors[0].description};
            }
        }
    };

    return (
        <>
            <div className="wrapper">
                <div className="user-settings-container">
                    <Cover imgPath={IMAGES.default_cover} size="big" />
                    <div className="user-settings-info">
                        <h1>settings</h1>
                        <div className='user-settings-info-container'>
                            <form className="user-settings-form">
                                <div className="user-settings-form-group">
                                    <label htmlFor="email">Email</label>
                                    <Input type="text" id="email"
                                           onChange={(e) => setEmail(e.target.value)} required />
                                </div>
                                <div className="user-settings-form-group">
                                    <label htmlFor="old-password">Old Password</label>
                                    <Input type="text" id="old-password"
                                           onChange={(e) => setOldPassword(e.target.value)} required />
                                </div>
                                <div className="user-settings-form-group">
                                    <label htmlFor="new-password">New Password</label>
                                    <Input type="text" id="new-password"
                                           onChange={(e) => setNewPassword(e.target.value)} required />
                                </div>
                                {validationError &&
                                    <p className="user-settings-error">Incorrect password</p>
                                }
                                <div>
                                    <Button text={'save'} round={'true'} color={'orange'} onClick={handleChange}/>
                                </div>
                                <span className='user-settings-form-delete-button'>Delete account</span>
                            </form>
                            <div className='user-settings-info-button'>
                                <Button text={'change'} color={'yellow'} iconPath={ICONS.binoculars} />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}
