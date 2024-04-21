import "./CheckoutPage.css";
import {useState} from "react";
import {Button} from "../../../components/UI/Button/Button.jsx";
import {Banner} from "../../../components/UI/Banner/Banner.jsx";
import {Input} from "../../../components/UI/Input/Input.jsx";
import PickUpPointModal from './../PickUpPointModal/PickUpPointModal.jsx';
import { Link } from "react-router-dom";

const CheckoutPage = () => {
    const [isOpen, setIsOpen] = useState(false);

    let totalPrice = 0;
    const goods = [
        {name: 'Товар 1', amount: 1, price: 1},
        {name: 'Товар 2', amount: 2, price: 1},
        {name: 'Товар 3', amount: 3, price: 1},
        {name: 'Товар 4', amount: 4, price: 1},
    ]
    goods.forEach(function (item){
        totalPrice += item.amount * item.price;
    })

    const openModal = () => setIsOpen(true);
    const closeModal = () => setIsOpen(false);

    return (
        <>
            <div className={'container-checkout'}>
                <div className={'title-checkout'}>
                    <h1>CHECKOUT</h1>
                </div>
                <Banner>
                    <div className={'label-input-checkout'} onClick={openModal}>
                        <label className={'label-checkout'} htmlFor={'address'}>Enter your email</label>
                        <Input className="input-checkout" id="address" placeholder="Type the address" type="text"/>
                    </div>
                    <div className={'goods-list-checkout-container'}>
                        {goods.map((item, index) => (
                            <p key={index}>{item.name}  кол-во: {item.amount}   цена: ${item.price}</p>
                        ))}
                    </div>
                    <div className={'total-checkout'}>
                        <span className={'subtotal-checkout'}>Subtotal:</span>
                        <div className={'total-button-checkout'}>
                            <Button color="yellow" round={"true"} text={`$${totalPrice}`}></Button>
                        </div>
                    </div>
                    <div className={'pay-button-checkout'}>
                        <Link to="/success" style={{textDecoration: 'none'}}>
                            <Button color="orange" round={"true"} text={"Pay with stripe"}></Button>
                        </Link>
                    </div>
                </Banner>
            </div>
            <PickUpPointModal isOpen={isOpen} onClose={closeModal}></PickUpPointModal>
        </>
    );
}

export default CheckoutPage