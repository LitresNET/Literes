import {Button} from "../../../components/UI/Button/Button.jsx";
import IMAGES from "../../../assets/images.jsx";
import ICONS from "../../../assets/icons.jsx";
import "../RedirectPageStyles.css";
import {Link} from "react-router-dom";

function OrderSuccessPage() {
    return(
        <div className="redirect-page">
            <div className="redirect-info">
                <img src={IMAGES.success_redirect} alt="ok"/>
                <h1>Thank you for your order!</h1>
                <p>Check your e-mail inbox for the receipt</p>
                <Link to={"/"}>
                    <Button round={"true"} color={"yellow"} text={"Continue shopping"} iconpath={ICONS.shopping_cart}/>
                </Link>
            </div>
        </div>
    );
}

export default OrderSuccessPage;