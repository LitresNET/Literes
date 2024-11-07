import {Button} from "../../../components/UI/Button/Button.jsx";
import {Input} from "../../../components/UI/Input/Input.jsx";
import IMAGES from "../../../assets/images.jsx";
import ICONS from "../../../assets/icons.jsx";
import "../RedirectPageStyles.css";
import {Link} from "react-router-dom";

function NotFoundPage() {
    return(
        <div className="redirect-page">
            <div className="redirect-info">
                <img src={IMAGES.error_redirect} alt="err"/>
                <h1>Opps! This page don’t exist</h1>
                <p>Please, return to one page existent</p>
                <Link to={"/"}><Button round={"true"} color={"yellow"} text={"Go back to home"} /></Link>
                <Input type={"checkbox"} iconPath={ICONS.github_logo}>
                    <p>You are Dev? Register an issue on github, if you want contribute to this project.</p>
                </Input>
            </div>
        </div>
    );
}

export default NotFoundPage;