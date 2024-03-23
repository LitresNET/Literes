import React from "react";

/// Принимает: <br/>
/// path : string - путь к иконке <br/>
/// size : string - размер ["default", "mini", "custom", "[any]" = "default"]<br/>
/// width : number - ширина (= высота, учитывается только при size = "custom")<br/>
export function Icon(props) {
    const {width, path, size} = props;

    let w;
    switch(size) {
        case "default":
            w = "25px"
            break;
        case "mini":
            w = "16px";
            break;
        case "custom":
            w = width + "px";
            break;
        default:
            w = "25px";
            break;
    }
    let h = w;

    return (
        <>
            <div className="icon" style={{maxWidth : w, maxHeight: h}} {...props}>
                <img src={path} alt=""/>
            </div>
        </>
    )
}