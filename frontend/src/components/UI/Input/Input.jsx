import './Input.css';
import React, { useState } from "react";
import { Icon } from "../Icon/Icon";
import ICONS from "../../../assets/icons.jsx";

/// Input, с настройкой стилей зависящей от поля type
export function Input({type, children, id, iconpath, ...rest}) {

    const [amount, setAmount] = useState(1);

    const trySetAmount = (n) => {
        if (n <= 0) {
            setAmount(1);
        } else if (n > 99) {
            setAmount(99);
        } else if (Number.parseInt(n).toString().length !== n.toString().length) {
            setAmount(1);
        } else {
            setAmount(Number.parseInt(n));
        }
    };

    switch (type) {
        case 'text':
            return (
                <>
                    <div className="input-wrapper">
                        <div className="input-text">
                            <input type={type} id={id} {...rest}/>
                        </div>
                    </div>
                </>
            )
        case 'number':
            return (
                <>
                    <div className="input-wrapper">
                        <div className="input-number">
                            <Icon path={ICONS.minus_circle} onClick={() => trySetAmount(amount - 1)}/>
                            <input type="number" id={id} value={amount} onChange={(e) => trySetAmount(e.target.value)} {...rest}/>
                            <Icon path={ICONS.plus_circle} onClick={() => trySetAmount(amount + 1)}/>
                        </div>
                    </div>
                </>
            )
        case 'checkbox':
            return (
                <>
                    <div className="input-wrapper">
                        <label className="input-checkbox">
                            <input type="checkbox" id={id} {...rest}/>
                            <div className="custom-checkbox">
                                {iconpath === null || iconpath === undefined
                                    ? "" : <Icon className="custom-checkbox-icon" path={iconpath} size={"mini"}/>}
                            </div>
                            <div className="custom-checkbox-content">
                                {children}
                            </div>
                        </label>
                    </div>
                </>
            )
        case 'password':
            return (
                <>
                    <div className="input-wrapper">
                        <div className="input-text">
                            <input type="password" id={id} {...rest}/>
                        </div>
                    </div>
                </>
            )
    }
}
