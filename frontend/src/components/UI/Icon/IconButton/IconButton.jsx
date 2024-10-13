import {Icon} from "../Icon.jsx";
import PropTypes from "prop-types";

/// Получает: <br/>
/// href : string - ссылка на страницу <br/>
/// остальные свойства передаются в тег Icon
export function IconButton (props) {
    IconButton.propsTypes = {
        href: PropTypes.string.isRequired
    }
    const { href, ...rest } = props;

    //Засунул data-testid прямо в компонент, т.к. иначе нельзя было протестить ссылку
    return (
        <>
            <a href={href} data-testid='icon-button-link'>
                <Icon {...rest}/>
            </a>
        </>
    );
}