// TODO: сверстать компонент отображающий описание - некий большой текст

/// Блок описания - принимает что угодно (включая html)
export function description(content) {
    return createDescription(content, "")
}

/// Блок описания c маленькими отступами - принимает что угодно (включая html)
export function descriptionMini(content) {
    return createDescription(content, "description-mini")
}

/// Блок описания c тенью - принимает что угодно (включая html)
export function descriptionShadow(content) {
    return createDescription(content, "banner-shadow")
}

/// Блок описания c тенью и маленькими отступами - принимает что угодно (включая html)
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