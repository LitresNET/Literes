import './BookSettingsPage.css'
import IMAGES from '../../../assets/images'
import { Cover } from '../../../components/Cover/Cover'
import { Input } from '../../../components/UI/Input/Input'
import { Button } from '../../../components/UI/Button/Button'

export default function BookSettingsPage() {
    return (
        <>
            <div className="wrapper">
                <div className="book-settings-container">
                    <Cover imgPath={IMAGES.default_cover} size="big" />
                    <div className="book-settings-info">
                        <h1>settings</h1>
                        <div className='book-settings-info-container'>
                            <form className="book-settings-form">
                                <div className="book-settings-form-group">
                                    <label htmlFor="email">Title</label>
                                    <Input type="text" id="email" required />
                                </div>
                                <div className="book-settings-form-group">
                                    <label htmlFor="Autor">Autor</label>
                                    <Input type="text" id="Autor" required />
                                </div>
                                <div className="book-settings-form-group">
                                    <label htmlFor="Description">Description</label>
                                    <Input type="textarea" id="Description" required />
                                </div>
                                <div className="book-settings-form-group book-settings-input-price">
                                    <label htmlFor="Price">Price</label>
                                    <Input type="text" id="Price" required />
                                </div>
                                <div>
                                    <Button text={'save'} round={'true'} color={'orange'} />
                                </div>
                                <span className='user-settings-form-delete-button'>Delete book</span>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}
