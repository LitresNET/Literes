import './Tag.css';
import React from 'react';
import {Icon} from "../Icon/Icon.jsx";
import ICONS from "../../../assets/icons.jsx";

/// Принимает: <br/>
/// status : ["success", "failure"] - статус действия <br/>
/// actionDescription : string - описание действия (что произошло)
export function Tag(props) {
    const {status, actiondescription, className} = props;
    let se;
    switch(status) {
        case "success":
            se = {
                className : "tag-success",
                iconpath : ICONS.check_circle,
                messagePrefix : "Success to: "
            };
            break;
        case "failure":
            se = {
                className : "tag-failure",
                iconpath : ICONS.circle_wavy_warning,
                messagePrefix : "Failed to: "
            };
            break;
        default:
            se = {
                className : "<empty>",
                iconpath : "<empty>",
                messagePrefix : "<empty>"
            }
    }

    let classes = "border-solid tag " + se.className + " " + className;

    return (
        <>
            <div className={classes} {...props}>
                <Icon path={se.iconpath}/>
                <p style={{marginLeft : "10px"}}>{se.messagePrefix + actiondescription}</p>
            </div>
        </>
    )
}