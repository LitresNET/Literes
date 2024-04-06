import './Quotation.css';
import {Description} from "../Description/Description.jsx";

/// Принимает: <br/>
/// Все параметры пересылаются в Description с size="mini"
export function Quotation(props) {
    return (
        <>
            <Description size={"mini"} {...props}/>
        </>
    )
}