// TODO: Заменить заглушки
import './BookCard.css';
import {Cover} from "../Cover/Cover.jsx";
import IMAGES from "../../assets/images.jsx";
import {Button} from "../UI/Button/Button.jsx";
import ICONS from "../../assets/icons.jsx";

/// Принимает: <br/>
/// bookId : number - id книги для отображения, остальные данные будут доставаться с сервера
export function BookCard(props) {
    const { bookId } = props;

    // Сделать получение данных о карточке с бека и использование их в генерации контента

    return (
        <>
            <div className="bookcard">
                <Cover imgPath={IMAGES.avatar_none}/>
                <div className="bookcard-buttons">
                    <div className="bookcard-button-row">
                        <Button text={"$30.00"} onClick={() => (alert("Заглушка!"))} round={"true"}/>
                        <Button iconpath={ICONS.bookmark_simple} onClick={() => (alert("Заглушка!"))} round={"true"}/>
                    </div>
                    <div className="bookcard-button-row">
                        <Button onClick={() => (alert("Заглушка!"))} iconpath={ICONS.shopping_cart}/>
                    </div>
                </div>
            </div>
        </>
    )
}
