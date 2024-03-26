import './Input.css';
import React, {useState} from "react";
import {Icon} from "../Icon/Icon";
import ICONS from "../../../assets/icons.jsx";

/// Input, с настройкой стилей зависящей от поля type
export function Input(props) {
    let {type} = props

    const [amount, setAmount] = useState(1);

    const trySetAmount = (n) => {
        if (n <= 0) {
            setAmount(1);
        } else if (n > 99) {
            setAmount(99);
        } else if (Number.parseInt(n).toString().length !== n.toString().length) {
            setAmount(1)
        } else {
            setAmount(Number.parseInt(n));
        }
    }

    switch (type) {
        case 'text':
            return (
                <>
                    <input {...props}/>
                </>
            )
        case 'number':
            return (
                <>
                    <div className="input-wrapper">
                        <Icon path={ICONS.minus_circle} onClick={() => trySetAmount(amount - 1)}/>
                        <input type="number" value={amount} onChange={(e) => trySetAmount(e.target.value)} {...props}/>
                        <Icon path={ICONS.plus_circle} onClick={() => trySetAmount(amount + 1)}/>
                    </div>
                </>
            )
    }
}