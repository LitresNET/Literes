// TODO: реализовать компонент карточки товара в корзине.
// Количество выбранного товара можно изменять
import {Banner} from "../UI/Banner/Banner";
import {Cover} from "../Cover/Cover";
import {Input} from "../UI/Input/Input.jsx";
import ICONS from "../../assets/icons.jsx";
import IMAGES from "../../assets/images.jsx";
import {IconButton} from "../UI/Icon/IconButton/IconButton.jsx";

/// Принимает: <br/>
/// bookId : number - id книги, остальные данные достаёт с сервера
export function CartItemCard(props) {
    const {bookId} = props;

    // Доставать все данные книги по её id

    return (
        <>
            <Banner padding="24px 26px">
                <div className="cart-item-card">
                    <Cover imgPath={IMAGES.library} size="custom" width="140" multiplier={1.2}/>
                    <div className="cart-item-card-info">
                        <div className="cart-item-card-group">
                            <div className="cart-item-card-row">
                                <p>Book name...</p>
                                <p className="price">$30.00</p>
                            </div>
                            <div className="cart-item-card-row">
                                <p className="cart-item-card-info-author">Author name...</p>
                            </div>
                        </div>
                        <div className="cart-item-card-row">
                            <Input type="number"/>
                            <IconButton path={ICONS.trash} onClick={() => alert("Заглушка!")} href=""/>
                        </div>
                    </div>
                </div>
            </Banner>
        </>
    )
}