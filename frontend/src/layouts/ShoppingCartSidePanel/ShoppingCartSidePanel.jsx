import {Banner} from "../../components/UI/Banner/Banner.jsx";
import {Button} from "../../components/UI/Button/Button.jsx";
import {CartItemCard} from "../../components/CartItemCard/CartItemCard.jsx";
import ICONS from "../../assets/icons.jsx";
import {IconButton} from "../../components/UI/Icon/IconButton/IconButton.jsx";
import "./ShoppingCartSidePanel.css";
import { Link } from "react-router-dom";

const ShoppingCartSidePanel = ({isOpen, handleClose}) => {
    if (!isOpen) return null;

    const handleOutsideClick = (event) => {
        if (event.target.classList.contains('shopping-cart-side-panel-darker')) {
            handleClose();
        }
    };

    return (
        <>
            <div className={'shopping-cart-side-panel-darker'} onClick={handleOutsideClick}>
                <div className={'shopping-cart-side-panel-container'}>
                    <div className={'container-top-shopping-cart-side-panel'}>
                        <div onClick={handleClose} style={{ cursor: 'pointer' }}>
                            <IconButton path={ICONS.caret_left} size="custom" width="32px"/>
                        </div>
                        <p>Your Cart<span>(02 items)</span></p>
                    </div>

                    <div className={'cart-item-list-shopping-cart-side-panel'}>
                        <div className={'cart-item-shopping-cart-side-panel'}>
                            <CartItemCard/>
                        </div>
                        <div className={'cart-item-shopping-cart-side-panel'}>
                            <CartItemCard/>
                        </div>
                    </div>
                    <div className={'payment-container-shopping-cart-side-panel'}>
                        <div className={'total-shopping-cart-side-panel'}>
                            <span className={'subtotal-shopping-cart-side-panel'}>Subtotal:</span>
                            <div className={'total-button-shopping-cart-side-panel'}>
                                <Button color="yellow" round={"true"} text={`$60`}></Button>
                            </div>
                        </div>
                        <div className={'pay-button-shopping-cart-side-panel'}>
                            <Link to="/checkout" style={{textDecoration: 'none'}}>
                                <Button color="orange" round={"true"} text={"Pay with stripe"}></Button>
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default ShoppingCartSidePanel