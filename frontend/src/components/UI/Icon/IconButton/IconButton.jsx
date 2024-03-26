import {Icon} from "../Icon.jsx";

/// Получает: <br/>
/// href : string - ссылка на страницу <br/>
/// остальные свойства передаются в тег Icon
export function IconButton (props) {
    const { href } = props;

    return (
        <>
            <a href={href}>
                <Icon {...props}/>
            </a>
        </>
    );
}