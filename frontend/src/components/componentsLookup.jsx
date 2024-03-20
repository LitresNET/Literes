import './componentsLookup.css'; // просто для красивого отображения страницы

import ICONS from "../assets/icons.jsx";
import IMAGES from "../assets/images.jsx";

import './component-assets.css'; // база - шрифты, цвета
import './UI/Icon/icon.css';
import './Cover/cover.css';
import './UI/Button/button.css';
import './UI/Banner/banner.css';
import './UI/Description/Description.css';
import './UI/Quotation/Quotation.css';
import './UI/Input/input.css';
import './UI/Tag/Tag.css';

import { coverBig, coverMini, cover, coverCustom } from "./Cover/cover.jsx";
import {
    button,
    orangeButton,
    roundedButton,
    roundedOrangeButton,
    roundedYellowButton,
    bigShadowButton,
    yellowButton
} from "./UI/Button/button.jsx";
import {Banner} from "./UI/Banner/banner.jsx";
import {IconButton} from "./UI/Icon/IconButton/iconButton.jsx";
import {description, descriptionMini, descriptionShadow, descriptionMiniShadow} from "./UI/Description/Description.jsx";
import {Input} from "./UI/Input/input.jsx";
import {quotation} from "./UI/Quotation/Quotation.jsx";
import {Tag} from "./UI/Tag/Tag";

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
                            <IconButton href={googleLink} path={ICONS.shopping_cart} size="mini"/>
                            <IconButton href={googleLink} path={ICONS.shopping_cart} size="default"/>
                            <IconButton href={googleLink} path={ICONS.shopping_cart} size="custom" width="70px"/>
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
                    <Banner>
                        <div style={{padding : "40px"}}>
                            <div>Привет!</div>
                            <Banner withShadow='true'>
                                <IconButton href={googleLink} path={ICONS.shopping_cart} size="default"/>
                            </Banner>
                        </div>
                    </Banner>
                </div>
                <div className="separator-example"></div>
                <div className="container">
                    <h3>Descriptions</h3>
                    <br/>
                    <div className="" style={{display: "flex", flexDirection: "column", justifyContent : "space-around"}}>
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
                <div className="separator-example"></div>
                <div className="container">
                    <h3>Inputs</h3>
                    <br/>
                    <div className="display-column">
                        <Input placeholder="Some number here" type="number"/>
                        <div>.</div>
                        <Input defaultValue="Hello!" placeholder="Some text here" type="text"/>
                    </div>
                </div>
                <div className="separator-example"></div>
                <div className="container">
                    <h3>Quotations</h3>
                    <br/>
                    <div className="display-row" style={{justifyContent : "space-evenly"}}>
                        {quotation("Привет, мир!", "(c) Любое приложение", "любая программа")}
                        {quotation("Привет, мир!", "(c) Любое приложение" , "любая программа")}
                    </div>
                </div>
                <div className="separator-example"></div>
                <div className="container">
                    <h3>Tags</h3>
                    <br/>
                    <div className="display-row">
                        <Tag status="success" actionDescription="create success tag!"/>
                        <Tag status="failure" actionDescription="create success tag!"/>
                    </div>
                </div>
            </div>
        </>
    );
}

export default componentsLookup;