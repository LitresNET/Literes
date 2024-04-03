import './Button.css';
import { Icon } from "../Icon/Icon.jsx";

/// Принимает: <br/>
/// text : string - текст кнопки <br/>
/// iconpath : string - путь к иконке кнопки <br/>
/// color : string - цвет кнопки ["yellow", "orange", "[any]" = "white"] <br/>
/// round : bool - закругление кнопки (более сильное) <br/>
/// shadow : bool - тень у кнопки <br/>
/// big : bool - немного увеличивает кнопку <br/>
/// также передаёт все параметры тега в тег button, поэтому возможна личная настройка
/// (при передаче своих классов - перезапишет все наложенные)
export function Button(props) {
    const {text, className, iconpath, color, round, shadow, big} = props
    let classes = "border-solid " +
        `button-${color} ` +
        (shadow ? "button-shadow " : " ") +
        (round ? "button-rounded " : " ") +
        (big ? "button-big " : " ") + " " +
        className;

    return (
        <>
            <button className={classes} {...props}>
                {text === "" || text == null ? null : <p>{text}</p>}
                {iconpath === "" || iconpath == null ? null : <Icon path={iconpath} size={"default"}/>}
            </button>
        </>
    )
}