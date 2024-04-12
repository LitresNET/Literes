// TODO: реализовать компонент выпадающего инпута поиска.
// При пользовательском вводе должна происходить фильтрация и результаты должны
// динамически подгружаться в окно результатов

import {Input} from "../UI/Input/Input";

export function DropDownInputSearch(props) {

    return (
        <>
            <Input type="text" placeholder="Пока вместо поиска везде пихается обычный Input" {...props}/>
        </>
    )
}