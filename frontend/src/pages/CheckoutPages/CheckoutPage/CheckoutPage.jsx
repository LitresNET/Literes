import "./CheckoutPage.css";
import {useState} from "react";
import {Button} from "../../../components/UI/Button/Button.jsx";
import {Banner} from "../../../components/UI/Banner/Banner.jsx";
import {Input} from "../../../components/UI/Input/Input.jsx";
import PickUpPointModal from './../PickUpPointModal/PickUpPointModal.jsx';
import configData from "./../../../../config.json";
import axiosToLitres from "./../../../hooks/useAxios.js"


const CheckoutPage = () => {
    const [isOpen, setIsOpen] = useState(false);
    const [address, setAddress] = useState();
    const paymentUrl = configData.PAYMENT_URL

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
    const onChoose = (point) => setAddress(point.address);

    const redirectToPayment = async () => {
        try {
            const response = await axiosToLitres.post('/order/create', {
                pickUpPointId: 1,
                books: goods,
                totalPrice: totalPrice
            });

            if (response.status === 200) {
                const paymentData = response.data;
                const queryParams = new URLSearchParams({
                    orderId: paymentData.orderId
                }).toString();

                window.location.href = `${paymentUrl}?${queryParams}`;
            } else {
                console.error('Ошибка при инициализации платежа:', response.data);
            }
        } catch (error) {
            console.error('Ошибка при отправке запроса:', error);
        }
    };

    return (
        <>
            <div className={'container-checkout'}>
                <div className={'title-checkout'}>
                    <h1>CHECKOUT</h1>
                </div>
                <Banner>
                    <div className={'label-input-checkout'} onClick={openModal}>
                        <label className={'label-checkout'} htmlFor={'address'}>Enter your email</label>
                        <Input className="input-checkout" id="address" placeholder="Type the address" type="text" value={address}/>
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
                        <Button color="orange" round={"true"} text={"Pay with stripe"} onClick={redirectToPayment}></Button>
                    </div>
                </Banner>
            </div>
            <PickUpPointModal isOpen={isOpen} onClose={closeModal} onChoose={onChoose}></PickUpPointModal>
        </>
    );
}

export default CheckoutPage