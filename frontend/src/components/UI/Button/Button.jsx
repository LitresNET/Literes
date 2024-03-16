// TODO: сверстать компоненты кнопки

import { icon } from "../Icon/Icon.jsx";

export function roundedYellowButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath,buttonType, "button-yellow button-rounded")
}

export function roundedOrangeButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath,buttonType, "button-orange button-rounded")
}

export function roundedButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text,onClick, iconPath,buttonType, "button-rounded")
}

export function bigShadowButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath, buttonType, "button-shadow button-big");
}

export function yellowButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath, buttonType, "button-yellow")
}

export function orangeButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath, buttonType, "button-orange")
}

export function button(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath, buttonType);
}

function createButton(text, onClick, iconPath = "", buttonType = "submit", classes = "") {
    return (
        <>
            <button className={classes} type={buttonType} onClick={onClick}>
                {text === "" ? null : <p>{text}</p>}
                {iconPath === "" ? null : icon(iconPath)}
            </button>
        </>
    )
}