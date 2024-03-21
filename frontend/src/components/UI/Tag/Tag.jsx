// TODO: сверстать компонент тега
import React from 'react';
import {Icon} from "../Icon/Icon.jsx";
import ICONS from "../../../assets/icons.jsx";

/// Принимает: <br/>
/// status : ["success", "failure"] - статус действия <br/>
/// actionDescription : string - описание действия (что произошло)
export class Tag extends React.Component {
    constructor(props) {
        super(props);
        this.status = props.status;
        this.actionDescription = props.actionDescription;
    }

    render() {
        const {status, actionDescription} = this.props;
        let se;
        switch(status) {
            case "success":
                se = {
                    className : "tag-success",
                    iconPath : ICONS.check_circle,
                    messagePrefix : "Success to: "
                };
                break;
            case "failure":
                se = {
                    className : "tag-failure",
                    iconPath : ICONS.circle_wavy_warning,
                    messagePrefix : "Failed to: "
                };
                break;
            default:
                se = {
                    className : "<empty>",
                    iconPath : "<empty>",
                    messagePrefix : "<empty>"
                }
        }
        return (
            <>
                <div className={"border-solid tag " + se.className}>
                    <Icon path={se.iconPath}/>
                    <p style={{marginLeft : "10px"}}>{se.messagePrefix + actionDescription}</p>
                </div>
            </>
        )
    }
}