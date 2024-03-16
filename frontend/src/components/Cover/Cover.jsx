// TODO: реализовать компонент обложки (книга или пользователь).
// При наведении пользователь может удалить или загрузить новый аватар или обложку (в случае с книгой)

/// Готовая обложка с шириной 280px и высотой 280px * 1.5 = 420px
export function coverBig(imgPath) {
    return createCover(imgPath, 280, 1.5);
}

/// Готовая обложка с шириной 200px и высотой 200px * 1.5 = 300px
export function cover(imgPath) {
    return createCover(imgPath, 200, 1.5);
}

/// Готовая обложка с одинаковой шириной и высотой равной 115px
export function coverMini(imgPath) {
    return createCover(imgPath, 115, 1);
}

/// Готовая обложка с кастомной шириной и дефолтным множителем высоты равным 1.5
export function coverCustom(imgPath, width, multiplier = 1.5) {
    return createCover(imgPath, width, multiplier);
}


function createCover(imgPath, width, multiplier) {
    let w = width.toString() + "px";
    let h = width * multiplier;

    return (
        <>
            <div className="cover" style={{width : w, height : h}}>
                <img src={imgPath} alt=""/>
            </div>
        </>
    );
}