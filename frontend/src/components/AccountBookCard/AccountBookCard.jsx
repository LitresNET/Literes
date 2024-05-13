import './AccountBookCard.css'
import { Cover } from '../Cover/Cover.jsx'
import { Button } from '../UI/Button/Button.jsx'
import PropTypes from 'prop-types'
import { Banner } from '../UI/Banner/Banner.jsx'
import ICONS from '../../assets/icons.jsx'
import { Link } from 'react-router-dom'
import {axiosToLitres} from "../../hooks/useAxios.js";

/// Принимает: <br/>
/// bookId : number - id книги для отображения, остальные данные будут доставаться с сервера
export function AccountBookCard(props) {
    AccountBookCard.propTypes = {
        id: PropTypes.number.isRequired,
        role: PropTypes.string.isRequired,
        author: PropTypes.string.isRequired,
        title: PropTypes.string.isRequired,
        imgPath: PropTypes.string.isRequired
    }

    const requestToDeleteBook = async (bookId) => {
        try {
            const response = await axiosToLitres.post(`api/book/${bookId}`);
        } catch (e) {
            return {result: null, error: e.response.data.errors[0].description};
        }
    }

    // Сделать получение данных о карточке с бека и использование их в генерации контента

    return (
        <>
            <div className="account-book-card-wrapper">
                <Cover imgPath={props.imgPath} />
                <Banner>"{props.title}" by {props.author}</Banner>

                {props.role === 'user' ? (
                    <Button text={'Read'} round={'true'} color={'yellow'} iconpath={ICONS.pencil} />
                ) : (
                    props.role ===
                    'publisher' && (
                        <div className="account-book-card-button">
                            <Link to={`book/${props.id}/settings`} state={{book: props}} style={{textDecoration: 'none'}}>
                                <Button
                                    text={'Edit'}
                                    round={'true'}
                                    color={'yellow'}
                                    iconpath={ICONS.pencil}
                                />
                            </Link>
                            <Button
                                text={'Delete'}
                                round={'true'}
                                color={'orange'}
                                iconpath={ICONS.trash}
                                onClick={() => requestToDeleteBook(props.id)}
                            />
                        </div>
                    )
                )}
            </div>
        </>
    )
}
