import ICONS from "../../assets/icons";
import { Banner } from "../UI/Banner/Banner";
import { Icon } from "../UI/Icon/Icon";
import './PickUpPointCard.css'

export function PickUpPointCard(props) {
    const {rating, address, workingHours, onClick} = props;
    const length = 5;
    const stars = Array.from({ length }, (_, index) => index );

    return (
        <>
            <Banner withshadow={"true"}>
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
        </>
    );
}