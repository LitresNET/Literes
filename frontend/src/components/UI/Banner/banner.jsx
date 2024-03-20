import React from "react";

/// Получает:<br/>
/// padding : string - строка с отступом в формате CSS-padding <br/>
/// withShadow : bool - нужна ли тень
export class Banner extends React.Component {
    constructor(props) {
        super(props);
        this.withShadow = props.withShadow;
        this.padding = props.padding;
    }

    render() {
        const {children, padding, withShadow} = this.props;
        return (
            <>
                <div className={"banner " + withShadow ? "banner-shadow" : ""} style={{padding: padding}}>
                    {children}
                </div>
            </>
        )
    }
}