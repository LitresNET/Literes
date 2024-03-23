/// Принимает: <br/>
/// id : string - id элемента (html-свойство id)
export function Checkbox(props) {
    const { children, id } = props;

    return (
        <>
            <input type="checkbox" className="custom-checkbox" id={id}/>
            <label htmlFor={id}>
                <div className="custom-checkbox-label">
                    {children}
                </div>
            </label>
        </>
    );
}