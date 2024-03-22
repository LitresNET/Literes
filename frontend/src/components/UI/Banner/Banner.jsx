/// Получает:<br/>
/// withshadow : bool - нужна ли тень
export function Banner(props) {
    const {children, withshadow} = props;
    return (
        <>
            <div className={"banner " + (withshadow ? "banner-shadow" : "")}>
                {children}
            </div>
        </>
    );
}