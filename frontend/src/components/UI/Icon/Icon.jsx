import './Icon.css';

/// Принимает: <br/>
/// path : string - путь к иконке <br/>
/// size : string - размер ["default", "mini", "custom", "[any]" = "default"]<br/>
/// width : number - ширина (= высота, учитывается только при size = "custom")<br/>
export function Icon({width, path, size, className, ...rest}) {
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
            <div className={"icon " + className}  {...rest}>
                <img src={path} style={{width : w, height : h}} alt=""/>
            </div>
        </>
    )
}