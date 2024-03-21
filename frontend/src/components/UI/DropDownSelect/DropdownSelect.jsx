// TODO: надо исправить
import React, {useRef, useState} from "react";
import {useClickOutside} from "../../../hooks/useClickOutside.js";
import {Icon} from "../Icon/Icon";
import ICONS from "../../../assets/icons.jsx";

/// Внутри используется НЕ select и работает вообще не очень (надо исправить)
export function DropdownSelect(props) {
    const { children, selectgroupname } = props;

    const [selectedValue, setSelectedValue] = useState(selectgroupname);
    const [isOpen, setOpen] = useState(false);
    const [isSelectActive, setSelectActive] = useState(false);
    const menuRef = useRef(null);

    useClickOutside(menuRef, () => {
        if (isOpen)
            setTimeout(() => setOpen(false), 100);
    });

    const updatedChildren = children.map((c) =>
        <>
            <p className={"dropdown-item"} onClick={(c) => selectValue(c)}>
                {c}
            </p>
        </>
    )

    function openDropDown() {
        setSelectActive(!isOpen);
        setOpen(!isOpen);
    }

    function selectValue(e) {
        setSelectedValue(e.target.children[0].textContent)
    }

    return (
        <>
            <div className={"border-solid dropdown-button " + (isSelectActive ? " dropdown-active" : "")} onClick={(e) => openDropDown(e)} {...props}>
                <div>{selectedValue}</div>
                <Icon size="mini" path={ICONS.caret_down}/>
            </div>
            <div className={"dropdown" + (isOpen ? " active" : "")}  ref={menuRef}>
                <div className={"dropdown-items-wrapper"}>
                    {updatedChildren}
                </div>
            </div>
        </>
    )
}