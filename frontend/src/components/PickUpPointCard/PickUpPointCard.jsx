import ICONS from "../../assets/icons";
import { Banner } from "../UI/Banner/Banner";
import { Icon } from "../UI/Icon/Icon";
import './PickUpPointCard.css'
import PropTypes from 'prop-types'

export function PickUpPointCard(props) {
    PickUpPointCard.propTypes = {
        rating: PropTypes.number.isRequired,
        address: PropTypes.string.isRequired,
        workingHours: PropTypes.string.isRequired,
        onClick: PropTypes.func.isRequired
    }

    const {rating, address, workingHours, onClick} = props;
    const length = 5;
    const stars = Array.from({ length }, (_, index) => index );

    return (
        <div className="">
            <Banner shadow={"true"}>
                <div className="point-container" onClick={onClick}>
                    <div className="point-stars">
                    {
                        stars.map((item, index) => (
                            <div key={index}>
                                {item > rating ? (
                                    <Icon path={ICONS.empty_star}/>
                                ) : (
                                    <Icon path={ICONS.filled_star}/>
                                )}
                            </div>
                        ))
                    }
                    </div>
                    <p className="point-address">{address}</p>
                    <p className="point-working-hours">{workingHours}</p>
                </div>
            </Banner>
        </div>
    );
}