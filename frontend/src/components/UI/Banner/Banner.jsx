import './Banner.css';

/// Получает:<br/>
/// withshadow : bool - нужна ли тень
export function Banner(props) {
    const {children, withshadow, className} = props;
    let classes = "banner " + (withshadow ? "banner-shadow" : "") + " " + className;
    return (
        <>
            <div className={classes} {...props}>
                {children}
            </div>
        </>
    );
}