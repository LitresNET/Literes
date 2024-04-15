import {Button} from "../../../components/UI/Button/Button.jsx";
import IMAGES from "../../../assets/images.jsx";
import ICONS from "../../../assets/icons.jsx";
import "../RedirectPageStyles.css";
import {Link} from "react-router-dom";

function UnauthorizedPage() {
    return(
        <>
            <div className="adaptive">
                <div className="redirect-page">
                    <div className="redirect-info">
                        <img src={IMAGES.cart_redirect} alt="ok"/>
                        <h1>We don't know you...</h1>
                        <p>In order to continue, please sign in</p>
                        <Link to="/signin">
                            <Button round={"true"} color={"yellow"} text={"Sign in"} iconpath={ICONS.sign_in}/>
                        </Link>
                    </div>
                </div>
            </div>
        </>
    );
}

export default UnauthorizedPage;