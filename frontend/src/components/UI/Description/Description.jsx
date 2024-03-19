// TODO: сверстать компонент отображающий описание - некий большой текст

/// Блок описания -
export function description(content) {
    return createDescription(content, "")
}

export function descriptionMini(content) {
    return createDescription(content, "description-mini")
}

export function descriptionShadow(content) {
    return createDescription(content, "banner-shadow")
}

export function descriptionMiniShadow(content) {
    return createDescription(content, "banner-shadow description-mini")
}

function createDescription(content, classes) {
    classes = "banner description " + classes;

    return (
        <>
            <div className={classes}>
                {content}
            </div>
        </>
    );
}