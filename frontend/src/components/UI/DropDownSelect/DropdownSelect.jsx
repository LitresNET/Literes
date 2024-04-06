// TODO: надо исправить
import './DropdownSelect.css';
import React, {useRef, useState} from "react";
import {useClickOutside} from "../../../hooks/useClickOutside.js";
import {Icon} from "../Icon/Icon";
import ICONS from "../../../assets/icons.jsx";

/// Внутри используется НЕ select и работает вообще не очень
export function DropdownSelect(props) {
    const { children, selectgroupname } = props;

    const [selectedValue, setSelectedValue] = useState(selectgroupname);
    const [isSelectActive, setSelectActive] = useState(false);
    const menuRef = useRef(null);

    useClickOutside(menuRef, () => {
        if (isSelectActive)
            setTimeout(() => setSelectActive(false), 100);
    });

    let counter = 0;
    const updatedChildren = children.map((c) => {
        let k = c;
        return (
            <p key={counter++} className={"dropdown-item"} onClick={(e) => selectValue(e)}>
                {k}
            </p>
        )
    });

    function openDropDown() {
        setSelectActive(!isSelectActive);
    }

    function selectValue(e) {
        setSelectedValue(e.target.children[0].textContent)
    }

    return (
        <>
            <div className="dropdown-wrapper">
                <div className={"border-solid dropdown-button " + (isSelectActive ? " dropdown-active" : "")} onClick={(e) => openDropDown(e)} {...props}>
                    <div>{selectedValue}</div>
                    <Icon size="mini" path={ICONS.caret_down}/>
                </div>
                <div className={"dropdown"}  ref={menuRef}>
                    <div className={"dropdown-items-wrapper"}>
                        {updatedChildren}
                    </div>
                </div>
            </div>
        </>
    )
}