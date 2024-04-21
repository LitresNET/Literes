import './BookSettingsPage.css'
import IMAGES from '../../../assets/images'
import { Cover } from '../../../components/Cover/Cover'
import { Input } from '../../../components/UI/Input/Input'
import { Button } from '../../../components/UI/Button/Button'
import { Link, useLocation } from 'react-router-dom';
import { useState } from 'react'

export default function BookSettingsPage() {
    let { state } = useLocation();
    const [book, setBook] = useState(state.book)

    const handleChange = (e) => {
        const { id, value } = e.target;
        setBook(prevBook => ({
            ...prevBook,
            [id]: value
        }));
    }

    return (
        <>
            <div className="wrapper">
                <div className="book-settings-container">
                    <Cover imgPath={state.book.imgPath ? state.book.imgPath : IMAGES.default_cover} size="big" />
                    <div className="book-settings-info">
                        <h1>settings</h1>
                        <div className='book-settings-info-container'>
                            <form className="book-settings-form">
                                <div className="book-settings-form-group">
                                    <label htmlFor="Title">Title</label>
                                    <Input type="text" id="title" required value={book.title} onChange={handleChange}/>
                                </div>
                                <div className="book-settings-form-group">
                                    <label htmlFor="Author">Author</label>
                                    <Input type="text" id="author" required value={book.author} onChange={handleChange}/>
                                </div>
                                <div className="book-settings-form-group">
                                    <label htmlFor="Description">Description</label>
                                    <Input type="textarea" id="Description" required onChange={handleChange}/>
                                </div>
                                <div className="book-settings-form-group book-settings-input-price">
                                    <label htmlFor="Price">Price</label>
                                    <Input type="text" id="Price" required onChange={handleChange}/>
                                </div>
                                <Link to="/account/publisher" style={{textDecoration: 'none'}}>
                                    <div>
                                        <Button text={'save'} round={'true'} color={'orange'} />
                                    </div>
                                </Link>
                                <span className='user-settings-form-delete-button'>Delete book</span>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}
