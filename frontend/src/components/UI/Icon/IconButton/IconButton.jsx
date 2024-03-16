/// Готовый контейнер с иконкой шириной 25px
export function iconButton(href, iconPath) {
    return createIconButton(href, iconPath, 25)
}


/// Готовый контейнер с иконкой шириной 16px
export function miniIconButton(href, iconPath) {
    return createIconButton(href, iconPath, 16)
}


/// Готовый контейнер с иконкой шириной width (в пикселях)
export function customIconButton(href, iconPath, width) {
    return createIconButton(href, iconPath, width)
}

function createIconButton(href, iconPath, width) {
    let w = width.toString() + 'px';
    let h = width * 1;

    return (
        <>
            <a href={href} className="icon" style={{width : w, height : h}}>
                <img src={iconPath} alt=""/>
            </a>
        </>
    )
}