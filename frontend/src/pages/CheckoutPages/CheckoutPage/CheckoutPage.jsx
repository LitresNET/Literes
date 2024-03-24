// TODO: сверстать страницу оформления заказа
import {useState} from "react";
import { Button } from '../../../components/UI/Button/Button';
import PickUpPointModal from './../PickUpPointModal/PickUpPointModal.jsx';
import './CheckoutPage.css'

export default function CheckoutPage() {
    const [isMapOpen, setOpen] = useState(false);
    const [selectedPoint, setPoint] = useState();

    const handleClose = () => {
        setOpen(false);
    };

    const handleOpen = () => {
        setOpen(true);
    };

    const handleChoose = (point) => {
        setPoint(point);
    };

    return(
        <>
            <div className="container">
                <h1>I'm a CheckoutPage</h1>
                <Button text={"Click me to open map"} onClick={handleOpen}></Button>
                <PickUpPointModal isOpen={isMapOpen} onClose={handleClose} onChoose={handleChoose}></PickUpPointModal>
                {selectedPoint && (
                    <>
                        Selecled point is {selectedPoint.address}
                    </>
                )}
            </div>
        </>
    )
}