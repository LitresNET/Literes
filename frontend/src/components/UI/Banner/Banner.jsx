import './Banner.css';
import PropTypes from "prop-types";

/// Получает:<br/>
/// withshadow : bool - нужна ли тень
export function Banner(props) {
    Banner.propsTypes = {
        withshadow: PropTypes.bool, //TODO: изменить на shadow
        className: PropTypes.string,
        children: PropTypes.node
    }
    const {children, withshadow, className, ...rest} = props;
    let classes = "banner" + (withshadow ? " banner-shadow" : "") + (className ? ` ${className}` : '');
    return (
        <>
            <div className={classes} {...rest}>
                {children}
            </div>
        </>
    );
}