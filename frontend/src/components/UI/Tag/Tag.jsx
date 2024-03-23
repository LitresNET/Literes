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
        return (
            <>
                <div className={"border-solid tag " + se.className}>
                    <Icon path={se.iconpath}/>
                    <p style={{marginLeft : "10px"}}>{se.messagePrefix + actionDescription}</p>
                </div>
            </>
        )
    }
}