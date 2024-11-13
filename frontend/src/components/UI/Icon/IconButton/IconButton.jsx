import {Icon} from "../Icon.jsx";
import PropTypes from "prop-types";

/// Получает: <br/>
/// href : string - ссылка на страницу <br/>
/// остальные свойства передаются в тег Icon
export function IconButton ({href, ...rest}) {
    IconButton.propsTypes = {
        href: PropTypes.string.isRequired
    }
    //Засунул data-testid прямо в компонент, т.к. иначе нельзя было протестить ссылку
    return (
        <>
            <a href={href} data-testid='icon-button-link'>
                <Icon {...rest}/>
            </a>
        </>
    );
}