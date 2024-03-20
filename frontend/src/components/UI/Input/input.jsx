// TODO: сверстать компонент текстового инпута
import React from "react";

/// Input но с настройкой стилей зависящей от поля type
export class Input extends React.Component {
    constructor(props) {
        super(props)
        this.type = props.type
    }

    render() {
        let classes = ""
        switch (this.type) {
            case 'text':
                classes += "text-input";
        }

        return (
            <>
                <input {...this.props} className={classes}/>
            </>
        )
    }
}