import './AccountBookCard.css'
import { Cover } from '../Cover/Cover.jsx'
import { Button } from '../UI/Button/Button.jsx'
import PropTypes from 'prop-types'
import { Banner } from '../UI/Banner/Banner.jsx'
import ICONS from '../../assets/icons.jsx'
import { Link } from 'react-router-dom'
import {useEffect, useState} from "react";
import {axiosToLitres} from "../../hooks/useAxios.js";
import {toast} from "react-toastify";
import IMAGES from "../../assets/images.jsx";

/// Принимает: <br/>
/// bookId : number - id книги для отображения, остальные данные будут доставаться с сервера
export function AccountBookCard(props) {
    //TODO: жесть а че пропсам можно required и даже типы указывать? надо это и с другими будет сделать
    AccountBookCard.propTypes = {
        bookId: PropTypes.number.isRequired,
        role: PropTypes.string.isRequired,
        /*
        author: PropTypes.string,//.isRequired,
        title: PropTypes.string,//.isRequired,
        imgPath: PropTypes.string//.isRequired */
    }

    const [data, setData] = useState(null);

    const [setErrorToast] = useState(null);
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axiosToLitres.get(`/book/${props.bookId}`);
                setData(response.data);
            } catch (error) {
                setErrorToast( () => toast.error('Account Book Card: '+error.message,
                    {toastId: "AccountBookCardError"}));
            }
        };
        fetchData();

    }, []);

    return (
        <>
            <div className="account-book-card-wrapper" {...props}>
                <Cover imgPath={!data?.coverUrl ? IMAGES.default_cover : data.coverUrl } link={`/book/${props.bookId}`} />
                <div className="account-book-card-name">
                    <Banner>
                        <p>"{data?.name}" by {data?.author}</p>
                    </Banner>
                </div>

                {props.role === 'user' ? (
                    <Button text={'Read'} round={'true'} color={'yellow'} iconpath={ICONS.pencil} />
                ) : (
                    props.role ===
                    'publisher' && (
                        <div className="account-book-card-button">
                            <Link to={`book/${props.id ?? data?.id}/settings`} state={{book: props}} style={{textDecoration: 'none'}}>
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
