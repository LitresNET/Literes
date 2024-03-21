import React from 'react';
import {Icon} from "../Icon.jsx";

export class IconButton extends React.Component {
    constructor(href, path, size, width) {
        super(href, width);
    }

    render() {
        const path = this.props.path;

        return (
            <>
                <a href={path}>
                    <Icon {...this.props}/>
                </a>
            </>
        )
    }
}