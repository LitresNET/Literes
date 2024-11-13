import './Button.css';
import { Icon } from "../Icon/Icon.jsx";
import PropTypes from "prop-types";

/// Принимает: <br/>
/// text : string - текст кнопки <br/>
/// iconPath : string - путь к иконке кнопки <br/>
/// color : string - цвет кнопки ["yellow", "orange", "[any]" = "white"] <br/>
/// round : bool - закругление кнопки (более сильное) <br/>
/// shadow : bool - тень у кнопки <br/>
/// big : bool - немного увеличивает кнопку <br/>
/// также передаёт все параметры тега в тег button, поэтому возможна личная настройка
/// (при передаче своих классов - перезапишет все наложенные)
export function Button({text, name, className, iconPath, color, round, shadow, big, ...rest}) {
    Button.propsTypes = {
        text: PropTypes.string,
        name: PropTypes.string,
        className: PropTypes.string,
        iconPath: PropTypes.string,
        color: PropTypes.string,
        round: PropTypes.bool,
        shadow: PropTypes.bool,
        big: PropTypes.bool
    }
    let classes = "border-solid" +
        (color ? ` button-${color}` : "" +
        (shadow ? " button-shadow" : "") +
        (round ? " button-rounded" : "") +
        (big ? " button-big" : "") +
            (className ? ` ${className}` : ""));

    return (
        <>
            <button className={classes} aria-label={name} {...rest}>
                {text ? <p>{text}</p> : null }
                {iconPath  ? <Icon path={iconPath} size={"default"} /> : null }
            </button>
        </>
    )
}