import {Button} from "../../../components/UI/Button/Button.jsx";
import {Input} from "../../../components/UI/Input/Input.jsx";
import IMAGES from "../../../assets/images.jsx";
import ICONS from "../../../assets/icons.jsx";
import "../RedirectPageStyles.css";
import {Link} from "react-router-dom";

function ErrorPage() {
    return(
        <div className="redirect-page">
            <div className="redirect-info">
                <img src={IMAGES.error_redirect} alt="err"/>
                <h1>Something went wrong</h1>
                <p>Please, refresh the page or try again later</p>
                <Link to={"/"}><Button round={"true"} color={"yellow"} text={"Go back to home"} /></Link>
                <Input type={"checkbox"} iconpath={ICONS.github_logo}>
                    <p>You are Dev? Register an issue on github, if you want contribute to this project.</p>
                </Input>
            </div>
        </div>
    );
}

export default ErrorPage;