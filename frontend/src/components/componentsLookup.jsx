import './componentsLookup.css'; // просто для красивого отображения страницы

import ICONS from "../assets/icons.jsx";
import IMAGES from "../assets/images.jsx";

import './component-assets.css'; // база - шрифты, цвета

import {Button} from "./UI/Button/Button.jsx";
import {Banner} from "./UI/Banner/Banner.jsx";
import {IconButton} from "./UI/Icon/IconButton/IconButton.jsx";
import {Description} from "./UI/Description/Description.jsx";
import {Input} from "./UI/Input/Input.jsx";
import {Quotation} from "./UI/Quotation/Quotation.jsx";
import {Tag} from "./UI/Tag/Tag";
import {DropdownSelect} from "./UI/DropDownSelect/DropdownSelect.jsx";
import {BookCard} from "./BookCard/BookCard";
import {Cover} from "./Cover/Cover.jsx";
import {CartItemCard} from "./CartItemCard/CartItemCard";
import {ReviewCard} from "./ReviewCard/ReviewCard";
import {SubscriptionCard} from "./SubscriptionCard/SubscriptionCard";
import {DropDownInputSearch} from "./DropDownInputSearch/DropDownInputSearch";

// Ссылка заглушка
const googleLink = "https://google.com/"

function ComponentsLookup() {

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
                        <Button text={"Adventure"} onClick={() => alert("Я самая обычная кнопка)")} iconpath={ICONS.binoculars} />
                        <Button text={"Adventure"} onClick={() => alert("Я жёлтая кнопка!")} color={"yellow"}/>
                        <Button text={"Adventure"} onClick={() => alert("Я оранжевая кнопка!")} color={"orange"}/>
                        <Button text={"Adventure"} onClick={() => alert("Я кнопка покруче, у меня уже есть тень!")} shadow="true" big={"true"}/>
                        <Button onClick={() => alert("Я круглая кнопка!")} iconpath={ICONS.binoculars} round={"true"}/>
                        <Button text={"Adventure"} onClick={() => alert("Я круглая жёлтая кнопка!")} round={"true"} color={"yellow"}/>
                        <Button text={"Adventure"} onClick={() => alert("Я круглая жёлтая кнопка!")} round={"true"} color={"orange"}/>
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
                        <Description>
                            <div>Привет!</div>
                            <Description size={"mini"}>
                                <div>Привет!</div>
                            </Description>
                        </Description>
                        <Description withshadow="true">
                            <div>Привет!</div>
                            <Description size={"mini"} withshadow={"true"}>
                                <div>Привет!</div>
                            </Description>
                        </Description>
                    </div>
                </div>
                <div id="inputs" className="container">
                    <h3>Inputs</h3>
                    <br/>
                    <div className="display-column">
                        <Input type="checkbox" iconpath={ICONS.binoculars}>
                            Key feature 1
                            <p className="price">$30,00</p>
                        </Input>
                        <Input type="checkbox" id={"hello"}>
                            Key feature 1
                        </Input>
                        <Input placeholder="0" type="number"/>
                        <Input defaultValue="Hello!" placeholder="Some text here" type="text"/>
                    </div>
                </div>
                <div id="quotations" className="container">
                    <h3>Quotations</h3>
                    <br/>
                    <div className="display-row" style={{justifyContent : "space-evenly"}}>
                        <Quotation>
                            <p className="quotation">[quotation]</p>
                            <p className="quotation-author">[author]<br/>[book]</p>
                        </Quotation>
                        <Quotation>
                            <p className="quotation">
                                "Привет, мир!Привет, мир!Привет, мир!Привет, мир!
                                Привет, мир!Привет, мир!"
                            </p>
                            <p className="quotation-author">
                                "(c) Любое приложение"
                                <br/>
                                "любая программа"
                            </p>
                        </Quotation>
                    </div>
                </div>
                <div id="tags" className="container">
                    <h3>Tags</h3>
                    <br/>
                    <div className="display-row">
                        <Tag status="success" actiondescription="create success tag!"/>
                        <Tag status="failure" actiondescription="create success tag!"/>
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

export default ComponentsLookup;