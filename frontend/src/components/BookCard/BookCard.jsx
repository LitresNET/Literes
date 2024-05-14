import './BookCard.css';
import {Cover} from "../Cover/Cover.jsx";
import IMAGES from "../../assets/images.jsx";
import {Button} from "../UI/Button/Button.jsx";
import ICONS from "../../assets/icons.jsx";
import {useEffect, useState} from "react";
import {axiosToLitres} from "../../hooks/useAxios.js";
import {toast} from "react-toastify";


/// Принимает: <br/>
/// bookId : number - id книги для отображения, остальные данные будут доставаться с сервера
export function BookCard(props) {
    const { bookId } = props; //TODO: добавить props'ы для заполнения компонента без обращения к бэку
    const [data, setData] = useState(null);

    const [setErrorToast] = useState(null);
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axiosToLitres.get(`/book/${bookId}`);
                setData(response.data);
            } catch (error) {
                setErrorToast( () => toast.error('Book Card: '+error.message,
                    {toastId: "BookCardError"}));
            }
        };
        fetchData();
    }, []);

    return (
        <>
            <div className="bookcard" {...props}>
                <Cover imgPath={data?.coverUrl === undefined || data?.coverUrl === null || data?.coverUrl === "" ?
                    //TODO: согласитесь, такие длинные проверки не оч красивые. Если бы у нас юрл аватарок повсеместно
                    //не равнялся бы "", то можно было бы обойтись коротким "data.coverUrd ??"
                    // (планирую заменить после того как сделаем нормальную seedData)
                    IMAGES.default_cover : data.coverUrl} link={`/book/${bookId}`}/>
                <div className="bookcard-buttons">
                    <div className="bookcard-button-row">
                        <p style={{width: "100%"}}>{data?.name}</p>
                    </div>
                    <div className="bookcard-button-row">
                        <Button text={data?.price + '$'} onClick={() => (alert("Заглушка!"))} round={"true"}/>
                        <Button iconpath={ICONS.bookmark_simple} onClick={() => (alert("Заглушка!"))} round={"true"}/>
                    </div>
                    <div className="bookcard-button-row">
                        <Button color={"orange"} onClick={() => (alert("Заглушка!"))} iconpath={ICONS.shopping_cart}/>
                    </div>
                </div>
            </div>
        </>
    )
}
