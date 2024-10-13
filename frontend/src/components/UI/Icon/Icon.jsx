import './Icon.css';
import PropTypes from "prop-types";

/// Принимает: <br/>
/// path : string - путь к иконке <br/>
/// size : string - размер ["default", "mini", "[any]" = "default" or width]<br/>
/// width : number - ширина (= высота, учитывается только при size = "custom")<br/>
/// alt : string - описание иконки <br/>
export function Icon({width, path, size, className, alt, ...rest}) {
    Icon.propsTypes = {
        path: PropTypes.string.isRequired,
        width: PropTypes.number,
        className: PropTypes.string,
        size: PropTypes.oneOf(['mini', 'default']),
        alt: PropTypes.string
    }

    let w;
    switch(size) {
        case "default":
            w = "25px"
            break;
        case "mini":
            w = "16px";
            break;
        default:
            w = width ? width + "px" : "25px";
            break;
    }
    let h = w;

    return (
        <>
            <div className={"icon" + (className ? ` ${className}` : "")}  {...rest}>
                <img src={path} style={{width : w, height : h}} alt={alt}/>
            </div>
        </>
    )
}