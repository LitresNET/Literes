import React from "react";

export class Checkbox extends React.Component {
    constructor(props) {
        super(props);
    }
    
    render() {
        const { children, id } = this.props;

        return (
            <>
                <input type="checkbox" className="custom-checkbox" id={id}/>
                <label htmlFor={id}>
                    <div className="custom-checkbox-label">
                        {children}
                    </div>
                </label>
            </>
        )
    }
}