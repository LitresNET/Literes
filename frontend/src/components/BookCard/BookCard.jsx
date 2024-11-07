import './BookCard.css';
import {Cover} from "../Cover/Cover.jsx";
import IMAGES from "../../assets/images.jsx";
import {Button} from "../UI/Button/Button.jsx";
import ICONS from "../../assets/icons.jsx";
import {useEffect, useState} from "react";
import {axiosToLitres} from "../../hooks/useAxios.js";
import {toast} from "react-toastify";
import PropTypes from "prop-types";
import {addBookToFavourites} from "../../features/addBookToFavourites.js";

//TODO: Реализовать логику добавления в корзину
/// Принимает: <br/>
/// bookId : number - id книги для отображения, остальные данные будут доставаться с сервера
export function BookCard(props) {
    BookCard.propTypes = {
        bookId: PropTypes.number.isRequired
    }

    const [data, setData] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axiosToLitres.get(`/book/${props.bookId}`);
                setData(response.data);
            } catch (error) {
                toast.error('Book Card: '+error.message,
                    {toastId: "BookCardError"});
            }
        };
        fetchData();
    }, []);

    return (
        <>
            <div className="bookcard" data-book-id={props.bookId}>
                <Cover imgPath={!data?.coverUrl ? IMAGES.default_cover : data.coverUrl} name={"book-cover"} link={`/book/${props.bookId}`}/>
                <div className="bookcard-name">
                    <p style={{width: "100%"}}>{data?.name}</p>
                </div>
                <div className="bookcard-buttons">
                    <div className="bookcard-button-row">
                        <Button text={data?.price + '$'} name={"book-price"} onClick={() => (alert("Заглушка!"))} round={"true"}/>
                        <Button iconPath={ICONS.bookmark_simple} name={"book-favourite"} onClick={async () =>
                            await addBookToFavourites(props.bookId)} round={"true"}/>
                    </div>
                    <div className="bookcard-button-row">
                        <Button color={"orange"} onClick={() => (alert("Заглушка!"))} iconPath={ICONS.shopping_cart}/>
                    </div>
                </div>
            </div>
        </>
    )
}
