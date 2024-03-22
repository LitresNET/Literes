//TODO: код говно, надо переделать на классы/пропсы/сделать возможность использовать их как теги

import { Icon } from "../Icon/Icon.jsx";

export function Button(props) {
    const {text, iconPath, color, round, shadow, big} = props
    let classes = "border-solid " +
        `button-${color} ` +
        (shadow ? "button-shadow " : " ") +
        (round ? "button-rounded " : " ") +
        (big ? "button-big " : " ");

    return (
        <>
            <button className={classes} {...props}>
                {text === "" ? null : <p>{text}</p>}
                {iconPath === "" ? null : <Icon path={iconPath}/>}
            </button>
        </>
    )
}