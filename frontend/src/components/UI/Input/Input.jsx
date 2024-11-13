import './Input.css';
import React, { useState } from "react";
import { Icon } from "../Icon/Icon";
import ICONS from "../../../assets/icons.jsx";
import PropTypes from "prop-types";

/// Input, с настройкой стилей зависящей от поля type
//TODO: отсуствует возможность убрать тень
//TODO: стоило бы сделать хук UseInput чтобы в компонентах каждый раз не прописывать вручную useState

export function Input({type, children, id, iconPath, ...rest}) {
    Input.propsTypes = {
        type: PropTypes.oneOf(["text", "number", "checkbox", "password"]).isRequired,
        children: PropTypes.node,
        id: PropTypes.number,
        iconPath: PropTypes.string
    }
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
                <div className="input-wrapper">
                    <div className="input-text">
                        <input type="text" id={id} {...rest}/>
                    </div>
                </div>
            )
        case 'number':
            return (
                <div className="input-wrapper">
                    <div className="input-number">
                        <Icon path={ICONS.minus_circle} alt={'icon-minus'} onClick={() => trySetAmount(amount - 1)}/>
                        <input type="number" id={id} value={amount} onChange={(e) => trySetAmount(e.target.value)} {...rest}/>
                        <Icon path={ICONS.plus_circle} alt={'icon-plus'} onClick={() => trySetAmount(amount + 1)}/>
                    </div>
                </div>
            )
        case 'checkbox':
            return (
                <div className="input-wrapper">
                    <label className="input-checkbox">
                        <input type="checkbox" id={id} {...rest}/>
                        <div className="custom-checkbox">
                            {iconPath ? <Icon className="custom-checkbox-icon" path={iconPath} size={"mini"} /> : ""}
                        </div>
                        <div className="custom-checkbox-content">
                            {children}
                        </div>
                    </label>
                </div>
            )
        case 'password':
            return (
                <div className="input-wrapper">
                    <div className="input-text">
                        <input type="password" id={id} {...rest}/>
                    </div>
                </div>
            )
    }
}
