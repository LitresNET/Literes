import './componentsLookup.css'; // просто для красивого отображения страницы

import ICONS from "../assets/icons.jsx";
import IMAGES from "../assets/images.jsx";

import './component-assets.css'; // база - шрифты, цвета
import './UI/Icon/Icon.css';
import './Cover/Cover.css';
import './UI/Button/Button.css';

import { iconButton, customIconButton, miniIconButton } from "./UI/Icon/IconButton/IconButton.jsx";
import { coverBig, coverMini, cover, coverCustom} from "./Cover/Cover.jsx";
import {
    button,
    orangeButton, roundedButton,
    roundedOrangeButton,
    roundedYellowButton,
    bigShadowButton,
    yellowButton
} from "./UI/Button/Button.jsx";
import COLORS from "../assets/colors.jsx";

// Ссылка заглушка
const googleLink = "https://google.com/"

function componentsLookup() {

    return (
        <>
            <div>
                <div className="container">
                    <h3>Fonts family</h3>
                    <br/>
                    <h1>Unica one</h1>
                    <p>Syne</p>
                </div>
                <div className="separator-example"></div>
                <div className="container">
                    <h3>Icons</h3>
                    <br/>
                    <div className="icons">
                        <div className="display-row">
                            {miniIconButton(googleLink, ICONS.shopping_cart)}
                            {iconButton(googleLink, ICONS.shopping_cart)}
                            {customIconButton(googleLink, ICONS.shopping_cart, 50)}
                        </div>
                    </div>
                </div>
                <div className="separator-example"></div>
                <div className="container">
                    <h3>Buttons</h3>
                    <br/>
                    <div className="display-column">
                        {button("Adventure", () => alert("Я самая обычная кнопка)"), ICONS.binoculars)}
                        {yellowButton("Adventure", () => alert("Я жёлтая кнопка!"))}
                        {orangeButton("Adventure", () => alert("Я оранжевая кнопка!"))}

                        {bigShadowButton("Adventure", () => alert("Я кнопка покруче, у меня уже есть тень!"))}

                        {roundedButton("", () => alert("Я круглая кнопка!"), ICONS.binoculars)}
                        {roundedYellowButton("Adventure", () => alert("Я круглая жёлтая кнопка!"))}
                        {roundedOrangeButton("Adventure", () => alert("Я круглая оранжевая кнопка!"))}

                    </div>
                </div>
                <div className="separator-example"></div>
                <div className="container">
                    <h3>Covers</h3>
                    <br/>
                    <div className="display-row">
                        {coverMini(IMAGES.avatar_none)}
                        {cover(IMAGES.avatar_none)}
                        {coverBig(IMAGES.avatar_none)}
                        {coverCustom(IMAGES.avatar_none, 240, 0.4)}
                    </div>
                </div>
                <div className="separator-example"></div>
                <div className="container">
                    <h3></h3>
                </div>
            </div>
        </>
    );
}

export default componentsLookup;