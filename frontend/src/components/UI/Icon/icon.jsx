/// Готовый контейнер с иконкой шириной 25px
export function icon(iconPath) {
    return createIcon(iconPath, 25)
}


/// Готовый контейнер с иконкой шириной 16px
export function miniIcon(iconPath) {
    return createIcon(iconPath, 16)
}


/// Готовый контейнер с иконкой шириной width (в пикселях)
export function customIcon(iconPath, width) {
    return createIcon(iconPath, width)
}

function createIcon(iconPath, width) {
    let w = width.toString() + 'px';
    let h = width * 1;

    // TODO: сделать вывод в тег svg, а не img - нужно будет трогать icons.jsx
    return (
        <>
            <div className="icon" style={{width : w, height : h}}>
                <img src={iconPath} alt=""/>
            </div>
        </>
    )
}