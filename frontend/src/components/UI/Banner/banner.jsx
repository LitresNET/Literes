

/// Баннер с нулевыми внутренними отступами (поддерживает все форматы CSS-padding)
export function banner(content, padding = "0") {
    return createBanner(content, padding, "")
}

/// Баннер с нулевыми внутренними отступами и тенью (поддерживает все форматы CSS-padding)
export function bannerShadow(content, padding = "0") {
    return createBanner(content, padding,"banner-shadow")
}

function createBanner(content, padding, classes) {
    classes = "banner " + classes;
    let p = padding;

    return (
        <>
            <div className={classes} style={{padding: p}}>
                {content}
            </div>
        </>
    );
}