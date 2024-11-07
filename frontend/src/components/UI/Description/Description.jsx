import './Description.css';
import PropTypes from "prop-types";

/// Принимает: <br/>
/// size : string - размер ["mini", "[any]" - default] <br/>
/// shadow : bool - тень <br/>
/// оставшиеся свойства передаются в тег div
export function Description({children, size, shadow, className, ...rest}) {
    Description.propTypes = {
        size: PropTypes.oneOf(["mini","[any]"]),
        shadow: PropTypes.bool,
        className: PropTypes.string,
        children: PropTypes.node
    }
    let classes =
        "banner description" +
        (size === "mini" ? " description-mini" : "") +
        (shadow ? " banner-shadow" : "") +
        (className ? ` ${className}` : "");

    return (
        <>
            <div className={classes} {...rest}>
                {children}
            </div>
        </>
    )
}