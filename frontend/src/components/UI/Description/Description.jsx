// TODO: сверстать компонент отображающий описание - некий большой текст


export function Description(props) {
    const {children, size, withshadow} = props
    let classes =
        "banner description " +
        (size === "mini" ? "description-mini " : "") +
        (withshadow ? "banner-shadow " : "");

    return (
        <>
            <div className={classes}{...props}>
                {children}
            </div>
        </>
    )
}