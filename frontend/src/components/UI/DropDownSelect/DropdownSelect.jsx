import './DropdownSelect.css';
import React, { useRef, useState } from "react";
import { useClickOutside } from "../../../hooks/useClickOutside";
import { Icon } from "../Icon/Icon";
import ICONS from "../../../assets/icons";

export function DropdownSelect({ selectgroupname, onChange, value, children }) {
    const [isOpen, setOpen] = useState(false);
    const menuRef = useRef(null);

    useClickOutside(menuRef, () => isOpen && setOpen(false));

    const openDropDown = () => setOpen(!isOpen);

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
                {isOpen && (
                    <div className="dropdown active" ref={menuRef}>
                        <div className="dropdown-items-wrapper">
                            {React.Children.map(children, (child, index) => (
                                <p key={index} className="dropdown-item" onClick={() => selectValue(child.props.children)}>
                                    {child.props.children}
                                </p>
                            ))}
                        </div>
                    </div>
                )}
            </div>
        </>
    );
}
