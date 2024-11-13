import './Banner.css';
import PropTypes from "prop-types";

/// Получает:<br/>
/// shadow : bool - нужна ли тень
export function Banner({children, shadow, className, ...rest}) {
    Banner.propsTypes = {
        shadow: PropTypes.bool,
        className: PropTypes.string,
        children: PropTypes.node
    }
    let classes = "banner" + (shadow ? " banner-shadow" : "") + (className ? ` ${className}` : '');
    return (
        <>
            <div className={classes} {...rest}>
                {children}
            </div>
        </>
    );
}