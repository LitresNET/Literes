import './SubscriptionFeature.css'

import { Banner } from '../UI/Banner/Banner'
import { Icon } from '../UI/Icon/Icon.jsx'
import ICONS from '../../assets/icons.jsx'
import { useState } from 'react'
import { Checkbox } from '../UI/Checkbox/Checkbox.jsx'
import PropTypes from 'prop-types'

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
                    {/* TODO: чекбокс никак не помечается, когда он выбран */}
                    <Checkbox>{props.name}</Checkbox>
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
