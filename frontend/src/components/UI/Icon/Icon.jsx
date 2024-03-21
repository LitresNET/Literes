import React from "react";

const Types = {
    default : "default",
    mini : "mini",
    custom : "custom"
}

export class Icon extends React.Component {
    constructor(props) {
        super(props);
        const {path, size = "default", width = null} = this.props
        this.path = path
        this.size = size
        this.width = width
    }

    render() {
        const {width, path, size} = this.props;
        let w;
        switch(size) {
            case Types.default || undefined || null:
                w = "25px";
                break;
            case Types.mini:
                w = "16px";
                break;
            case Types.custom:
                w = width;
                break;
            default:
                w = "25px";
                break;
        }
        let h = w;

        return (
            <>
                <div className="icon" style={{width : w, height: h}}>
                    <img src={path} alt=""/>
                </div>
            </>
        )
    }
}