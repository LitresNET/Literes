import './DropdownSelect.css';
import React, { useRef, useState } from "react";
import { useClickOutside } from "../../../hooks/useClickOutside";
import { Icon } from "../Icon/Icon";
import ICONS from "../../../assets/icons";

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

    const selectValue = (selectedText) => {
        onChange(selectedText); // Вызываем onChange с выбранным значением
        setOpen(false); // Закрываем дропдаун после выбора
    };

    return (
        <>
            <div className="dropdown-wrapper">
                <div className={"border-solid dropdown-button " + (isOpen ? " dropdown-active" : "")} onClick={openDropDown}>
                    <div>{value || selectgroupname}</div>
                    <Icon size="mini" path={ICONS.caret_down} />
                </div>
                <div className={"dropdown"}  ref={menuRef}>
                    <div className={"dropdown-items-wrapper"}>
                        {updatedChildren}
                    </div>
                )}
            </div>
        </>
    );
}
