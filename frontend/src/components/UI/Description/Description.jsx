/// Принимает: <br/>
/// size : string - размер ["mini", "[any]" - default] <br/>
/// withshadow : bool - тень <br/>
/// оставшиеся свойства передаются в тег div
export function Description(props) {
    const {children, size, withshadow} = props
    let classes =
        "banner description " +
        (size === "mini" ? "description-mini " : "") +
        (withshadow ? "banner-shadow " : "");

    return (
        <>
            <div className={classes} {...props}>
                {children}
            </div>
        </>
    )
}