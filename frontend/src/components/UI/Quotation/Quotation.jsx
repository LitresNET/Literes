// TODO: сверстать компонент карточки с цитатой

import {descriptionMini} from "../Description/Description.jsx";

export function quotation(content, author, book) {
    return createQuotation(content, author, book);
}

function createQuotation(content, author, book) {
    let descContent =
        <>
            <p className="quotation">{content}</p>
            <p className="quotation-author">{author}<br/>{book}</p>
        </>
    return (
        <>
            {descriptionMini(descContent)}
        </>
    )
}