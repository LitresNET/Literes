import './SubscriptionFeature.css'

import { Banner } from '../UI/Banner/Banner'
import { Icon } from '../UI/Icon/Icon.jsx'
import ICONS from '../../assets/icons.jsx'
import { useState } from 'react'
import PropTypes from 'prop-types'
import {Input} from "../UI/Input/Input.jsx";

export function SubscriptionFeature(props) {
    SubscriptionFeature.propTypes = {
        name: PropTypes.string.isRequired,
        description: PropTypes.string.isRequired
    }

    const [isOpen, setOpen] = useState(false)

    return (
        <>
            <div className='subscription'>
                <div className="subscription-title">
                    <Input type={"checkbox"}>{props.name}</Input>
                    <div className="subscription-open-button">
                        <Icon
                            path={ICONS.caret_down}
                            size="mini"
                            style={{ transform: isOpen ? 'rotateZ(-180deg)' : 'rotateZ(0)' }}
                            onClick={() => setOpen(!isOpen)}
                        />
                    </div>
                </div>

                <div className={'subscription-description' + (isOpen ? 'active' : '')}>
                    <Banner>{props.description}</Banner>
                </div>
            </div>
        </>
    )
}
