import './componentsLookup.css'; // просто для красивого отображения страницы

import ICONS from "../assets/icons.jsx";
import IMAGES from "../assets/images.jsx";

import './component-assets.css'; // база - шрифты, цвета
import './UI/Icon/icon.css';
import './Cover/cover.css';
import './UI/Button/button.css';
import './UI/Banner/banner.css';
import './UI/Description/Description.css';

import { iconButton, customIconButton, miniIconButton } from "./UI/Icon/IconButton/iconButton.jsx";
import { coverBig, coverMini, cover, coverCustom } from "./Cover/cover.jsx";
import {
    button,
    orangeButton, roundedButton,
    roundedOrangeButton,
    roundedYellowButton,
    bigShadowButton,
    yellowButton
} from "./UI/Button/button.jsx";
import {banner, bannerShadow} from "./UI/Banner/banner.jsx";
import {icon} from "./UI/Icon/icon.jsx";
import {description, descriptionMini, descriptionShadow, descriptionMiniShadow} from "./UI/Description/Description.jsx";

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
                    <h3>Banners</h3>
                    <br/>
                    {banner(
                        <>
                            <div style={{padding : "40px"}}>
                                <div>Привет!</div>
                                {bannerShadow(icon(ICONS.binoculars))}
                            </div>
                        </>
                    )}
                </div>
                <div className="separator-example"></div>
                <div className="container">
                    <h3>Descriptions</h3>
                    <br/>
                    <div className="" style={{display: "flex", flexDirection: "column"}}>
                        {description(
                            <>
                                <div>Привет!</div>
                                {descriptionMini(<><div>Привет!</div></>)}
                            </>
                        )}
                        {descriptionShadow(
                            <>
                                <div>Привет!</div>
                                {descriptionMiniShadow(<><div>Привет!</div></>)}
                            </>
                        )}
                    </div>
                </div>
            </div>
        </>
    );
}

export default componentsLookup;