import './UserSettingsPage.css'
import ICONS from '../../../assets/icons'
import IMAGES from '../../../assets/images'
import { Cover } from '../../../components/Cover/Cover'
import { Input } from '../../../components/UI/Input/Input'
import { Button } from '../../../components/UI/Button/Button'

export default function UserSettingsPage() {
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
                                    <Input type="text" id="email" required />
                                </div>
                                <div className="user-settings-form-group">
                                    <label htmlFor="old-password">Old Password</label>
                                    <Input type="text" id="old-password" required />
                                </div>
                                <div className="user-settings-form-group">
                                    <label htmlFor="new-password">New Password</label>
                                    <Input type="text" id="new-password" required />
                                </div>
                                <div>
                                    <Button text={'save'} round={'true'} color={'orange'} />
                                </div>
                                <span className='user-settings-form-delete-button'>Delete account</span>
                            </form>
                            <div className='user-settings-info-button'>
                                <Button text={'change'} color={'yellow'} iconpath={ICONS.binoculars} />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}
