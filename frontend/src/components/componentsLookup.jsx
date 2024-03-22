import './componentsLookup.css'; // просто для красивого отображения страницы

import ICONS from "../assets/icons.jsx";
import IMAGES from "../assets/images.jsx";

import './component-assets.css'; // база - шрифты, цвета
import './UI/Icon/Icon.css';
import './UI/Button/button.css';
import './UI/Banner/Banner.css';
import './UI/Description/Description.css';
import './UI/Quotation/Quotation.css';
import './UI/Input/Input.css';
import './UI/Tag/Tag.css';
import './UI/Checkbox/Checkbox.css';
import './UI/DropDownSelect/DropdownSelect.css';
import './Cover/Cover.css';
import './BookCard/BookCard.css';
import './CartItemCard/CartItemCard.css';
import './ReviewCard/ReviewCard.css';
import './SubscriptionCard/SubscriptionCard.css';

import {
    button,
    orangeButton,
    roundedButton,
    roundedOrangeButton,
    roundedYellowButton,
    bigShadowButton,
    yellowButton
} from "./UI/Button/button.jsx";
import {Banner} from "./UI/Banner/Banner.jsx";
import {IconButton} from "./UI/Icon/IconButton/IconButton.jsx";
import {description, descriptionMini, descriptionShadow, descriptionMiniShadow} from "./UI/Description/Description.jsx";
import {Input} from "./UI/Input/Input.jsx";
import {quotation} from "./UI/Quotation/Quotation.jsx";
import {Tag} from "./UI/Tag/Tag";
import {Checkbox} from "./UI/Checkbox/Checkbox.jsx";
import {DropdownSelect} from "./UI/DropDownSelect/DropdownSelect.jsx";
import {BookCard} from "./BookCard/BookCard";
import {Cover} from "./Cover/Cover.jsx";
import {CartItemCard} from "./CartItemCard/CartItemCard";
import {ReviewCard} from "./ReviewCard/ReviewCard";
import {SubscriptionCard} from "./SubscriptionCard/SubscriptionCard";
import {DropDownInputSearch} from "./DropDownInputSearch/DropDownInputSearch";

// Ссылка заглушка
const googleLink = "https://google.com/"

function componentsLookup() {

    return (
        <>
            <div>
                <div id="fonts" className="container">
                    <h3>Fonts family</h3>
                    <br/>
                    <h1>Unica one</h1>
                    <p>Syne</p>
                </div>
                <div id="icons" className="container">
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
                <div id="buttons" className="container">
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
                <div id="covers" className="container">
                    <h3>Covers</h3>
                    <br/>
                    <div className="display-row">
                        <Cover imgPath={IMAGES.avatar_none}/>
                        <Cover imgPath={IMAGES.avatar_none} size={"default"}/>
                        <Cover imgPath={IMAGES.avatar_none} size={"mini"}/>
                        <Cover imgPath={IMAGES.avatar_none} size={"big"}/>
                        <Cover imgPath={IMAGES.avatar_none} size={"custom"} width={150}/>
                        <Cover imgPath={IMAGES.avatar_none} size={"custom"} width={100} multiplier={3}/>
                    </div>
                </div>
                <div id="banners" className="container">
                    <h3>Banners</h3>
                    <br/>
                    <Banner>
                        <div>
                            <div>Привет!</div>
                            <Banner withshadow='true'>
                                <IconButton href={googleLink} path={ICONS.shopping_cart} size="default"/>
                            </Banner>
                        </div>
                    </Banner>
                </div>
                <div id="descriptions" className="container">
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
                <div id="inputs" className="container">
                    <h3>Inputs</h3>
                    <br/>
                    <div className="display-column">
                        <Input placeholder="0" type="number"/>
                        <div>.</div>
                        <Input defaultValue="Hello!" placeholder="Some text here" type="text"/>
                    </div>
                </div>
                <div id="quotations" className="container">
                    <h3>Quotations</h3>
                    <br/>
                    <div className="display-row" style={{justifyContent : "space-evenly"}}>
                        {quotation("Привет, мир!Привет, мир!Привет, мир!Привет, мир!Привет, мир!Привет, мир!", "(c) Любое приложение", "любая программа")}
                        {quotation("Привет, мир!", "(c) Любое приложение" , "любая программа")}
                    </div>
                </div>
                <div id="tags" className="container">
                    <h3>Tags</h3>
                    <br/>
                    <div className="display-row">
                        <Tag status="success" actionDescription="create success tag!"/>
                        <Tag status="failure" actionDescription="create success tag!"/>
                    </div>
                </div>
                <div id="checkboxes" className="container">
                    <h3>Checkboxes</h3>
                    <br/>
                    <div className="display-row">
                        <Checkbox id="creative">
                            [Some content in text or html]
                        </Checkbox>
                        <Checkbox id="creative1">
                            Key feature 1
                            <p className="price">$30,00</p>
                        </Checkbox>
                        <Checkbox id="creative2">
                            Key feature 2
                            <p className="price">$30,00</p>
                        </Checkbox>
                        <Checkbox id="creative3">
                            Key feature 3
                            <p className="price">$30,00</p>
                        </Checkbox>
                    </div>
                </div>
                <div id="to-refactor dropdown-select" className="container">
                    <h3>Drop down select</h3>
                    <br/>
                    <DropdownSelect selectgroupname="[selectgroupname]">
                        <option value="hello">Hello</option>
                        <option value="bye">Bye</option>
                    </DropdownSelect>
                </div>
                <div id="logic-based book-cards" className="container">
                    <h3>Book card</h3>
                    <br/>
                    <div className="display-row">
                        <BookCard bookId={20}/>
                        <BookCard />
                        <BookCard />
                        <BookCard />
                    </div>
                </div>
                <div id="logic-based cart-item-cards" className="container">
                    <h3>Card item cards</h3>
                    <br/>
                    <div className="display-column" style={{maxHeight : "300px", overflowX : "scroll"}}>
                        <CartItemCard />
                        <CartItemCard />
                        <CartItemCard />
                        <CartItemCard />
                    </div>
                </div>
                <div id="logic-based dropdown-input-search" className="container">
                    <h3>Dropdown input search</h3>
                    <br/>
                    <div className="display-column">
                        <DropDownInputSearch />
                    </div>
                </div>
                <div id="logic-based review-cards" className="container">
                    <h3>Review cards</h3>
                    <br/>
                    <div className="display-column">
                        <ReviewCard />
                        <ReviewCard />
                        <ReviewCard />
                    </div>
                </div>
                <div id="logic-based subscription-cards" className="container">
                    <h3>subscription cards</h3>
                    <br/>
                    <div className="display-row">
                        <SubscriptionCard />
                        <SubscriptionCard />
                        <SubscriptionCard />
                        <SubscriptionCard />
                        <SubscriptionCard />
                    </div>
                </div>
            </div>
        </>
    );
}

export default componentsLookup;