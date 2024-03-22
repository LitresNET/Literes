// TODO: сверстать компонент текстового инпута
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

    let classes = ""

    switch (type) {
        case 'text':
            return (
                <>
                    <input {...props} className={classes}/>
                </>
            )
        case 'number':
            return (
                <>
                    <div className="input-wrapper">
                        <Icon path={ICONS.minus_circle} onClick={() => trySetAmount(amount - 1)}/>
                        <input type="number" value={amount} onChange={(e) => trySetAmount(e.target.value)}/>
                        <Icon path={ICONS.plus_circle} onClick={() => trySetAmount(amount + 1)}/>
                    </div>
                </>
            )
    }
}