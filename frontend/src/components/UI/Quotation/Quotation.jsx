import {Description} from "../Description/Description.jsx";


export function Quotation(props) {
    return (
        <>
            <Description size={"mini"}>
                {props.children}
            </Description>
        </>
    )
}