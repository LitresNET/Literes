// TODO: Заменить заглушки
import {Cover} from "../Cover/Cover.jsx";
import IMAGES from "../../assets/images.jsx";
import {roundedButton, roundedYellowButton} from "../UI/Button/button.jsx";
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
                        {roundedButton("$30.00", () => "")}
                        {roundedButton("", () => (alert("Заглушка!")), ICONS.bookmark_simple)}
                    </div>
                    <div className="bookcard-button-row">
                        {roundedYellowButton("", () => (alert("Заглушка!")), ICONS.shopping_cart)}
                    </div>
                </div>
            </div>
        </>
    )
}
