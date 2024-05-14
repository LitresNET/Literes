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

/// Принимает: <br/>
/// bookId : number - id книги для отображения, остальные данные будут доставаться с сервера
export function AccountBookCard(props) {
    //TODO: жесть а че пропсам можно required и даже типы указывать? надо это и с другими будет сделать
    AccountBookCard.propTypes = {
        id: PropTypes.number.isRequired, //чзх нах три нижние required если мы получаем id и берём это через фронт?
        role: PropTypes.string.isRequired,
        author: PropTypes.string,//.isRequired,
        title: PropTypes.string,//.isRequired,
        imgPath: PropTypes.string//.isRequired не required, у user.favourites только id...
            //хотя вроде один хуй эти required не вызывают никаких ошибок и прочего так что пох
    }

    const [data, setData] = useState(null);

    const [setErrorToast] = useState(null);
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axiosToLitres.get(`/book/${props.id}`);
                setData(response.data);
            } catch (error) {
                setErrorToast( () => toast.error('Account Book Card: '+error.message,
                    {toastId: "AccountBookCardError"}));
            }
        };
        if(props.id !== null && props.id !== undefined && props.id !== 0) {
            fetchData();
        }
    }, []);
    //TODO: бля вообще наверное можно не быть тупым хуесосом не писать везде "props. ?? data." и просто все с data после fetch'a
    // записывать в props, да? ой сучеька бля пох завтра доделаем какой же ад невыносимый

    return (
        <>
            <div className="account-book-card-wrapper">
                <Cover imgPath={props.imgPath ?? data?.coverUrl} />
                <Banner>"{props.title ?? data?.coverUrl}" by {props.author ?? data?.author}</Banner>

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
                            />
                        </div>
                    )
                )}
            </div>
        </>
    )
}
