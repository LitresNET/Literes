/// Принимает: <br/>
/// imgPath : string - путь к обложке книги
/// size : string - размер : ["default", "mini", "big", "custom", "[any other]" = "default"]
/// multiplier : number - число (только при size = "custom")
/// width : number - число (только при size = "custom");
export function Cover(props) {
    const { imgPath, size, multiplier, width } = props;

    let w = 200;
    let h;
    let m = 1.5;
    switch (size) {
        case "big":
            w = 280;
            break;
        case "mini":
            w = 120;
            m = 1;
            break;
        case "custom":
            w = width === undefined || width === null
                ? w
                : width;
            m = multiplier === undefined || multiplier === null
                ? m
                : multiplier;
            break;
    }
    h = w * m + "px";
    w += "px";

    return (
        <>
            <div className="cover" style={{minWidth : w, height : h}}>
                <img src={imgPath} alt=""/>
            </div>
        </>
    );
}