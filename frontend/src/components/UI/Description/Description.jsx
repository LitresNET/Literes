import './Description.css';
import PropTypes from "prop-types";

/// Принимает: <br/>
/// size : string - размер ["mini", "[any]" - default] <br/>
/// withshadow : bool - тень <br/>
/// оставшиеся свойства передаются в тег div
export function Description(props) {
    Description.propTypes = {
        size: PropTypes.oneOf(["mini","[any]"]),
        withshadow: PropTypes.bool, //TODO: изменить на shadow
        className: PropTypes.string,
        children: PropTypes.node
    }
    const {children, size, withshadow, className, ...rest} = props
    let classes =
        "banner description" +
        (size === "mini" ? " description-mini" : "") +
        (withshadow ? " banner-shadow" : "") +
        (className ? ` ${className}` : "");

    return (
        <>
            <div className={classes} {...rest}>
                {children}
            </div>
        </>
    )
}