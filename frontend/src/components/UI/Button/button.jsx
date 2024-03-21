//TODO: код говно, надо переделать на классы/пропсы/сделать возможность использовать их как теги

import { Icon } from "../Icon/Icon.jsx";

/// Жёлтая кнопка с круглыми краями. При наведении - оранжевая
export function roundedYellowButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath,buttonType, "button-yellow button-rounded")
}

/// Оранжевая кнопка с круглыми краями. При наведении - жёлтая
export function roundedOrangeButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath,buttonType, "button-orange button-rounded")
}

/// Белая кнопка с круглыми краями. При наведении - жёлтая
export function roundedButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text,onClick, iconPath,buttonType, "button-rounded")
}

/// Белая большая кнопка с тенью. При наведении - жёлтая
export function bigShadowButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath, buttonType, "button-shadow button-big");
}

/// Жёлтая кнопка. При наведении - оранжевая
export function yellowButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath, buttonType, "button-yellow")
}

/// Оранжевая кнопка. При наведении - жёлтая
export function orangeButton(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath, buttonType, "button-orange")
}

/// Белая кнопка. При наведении жёлтая
export function button(text, onClick, iconPath = "", buttonType = "submit") {
    return createButton(text, onClick, iconPath, buttonType);
}

function createButton(text, onClick, iconPath = "", buttonType = "submit", classes = "") {
    classes = "border-solid " + classes
    return (
        <>
            <button className={classes} type={buttonType} onClick={onClick}>
                {text === "" ? null : <p>{text}</p>}
                {iconPath === "" ? null : <Icon path={iconPath}/>}
            </button>
        </>
    )
}