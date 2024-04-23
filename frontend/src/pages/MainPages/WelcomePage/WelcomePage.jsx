import IMAGES from "../../../assets/images.jsx";
import {DropDownInputSearch} from "../../../components/DropDownInputSearch/DropDownInputSearch";
import ICONS from "../../../assets/icons.jsx";
import {Button} from "../../../components/UI/Button/Button.jsx";
import './WelcomePage.css';
import {Icon} from "../../../components/UI/Icon/Icon.jsx";
import {Banner} from "../../../components/UI/Banner/Banner";
import {Cover} from "../../../components/Cover/Cover.jsx";
import {BookCard} from "../../../components/BookCard/BookCard";

function mainPage() {

    return (
        <>
                <div className="main-page">
                    <div className="head">
                        <div className="head-content">
                            <h1 className={"font-syne"}>What book you looking for?</h1>
                            <p>Explore our catalog and find your next read.</p>
                            <DropDownInputSearch />
                            <div className="head-button">
                                <Button text={"Explore"} iconpath={ICONS.binoculars} color={"yellow"} />
                                <div className={"separator"} />
                            </div>
                        </div>
                        <div className="head-image">
                            <img src={IMAGES.library} alt={"library-banner"}/>
                        </div>
                    </div>
                    <div className="fraction-trending-books">
                        <div className="fraction-title">
                            <h1>Trending Books</h1>
                            <div className="fraction-subtitle">
                                <Icon path={ICONS.sparkle} size={"mini"}/>
                                <p>Drag to explore</p>
                            </div>
                        </div>
                        <div className="trending-books">
                            <Banner>
                                <Cover imgPath={IMAGES.avatar_none}/>
                                <Cover imgPath={IMAGES.avatar_none}/>
                                <Cover imgPath={IMAGES.avatar_none}/>
                                <Cover imgPath={IMAGES.avatar_none}/>
                                <Cover imgPath={IMAGES.avatar_none}/>
                                <Cover imgPath={IMAGES.avatar_none}/>
                                <Cover imgPath={IMAGES.avatar_none}/>
                                <Cover imgPath={IMAGES.avatar_none}/>
                            </Banner>
                        </div>

                    </div>
                    <div className="fraction-categories">
                        <div className="category-title">
                            <Icon path={ICONS.folder_notch_open}/>
                            <h1>Categories</h1>
                        </div>
                        <div className="categories">
                            <Button text={"Adventure"} big={"true"} shadow={"true"}/>
                            <Button text={"Adventure"} big={"true"} shadow={"true"}/>
                            <Button text={"Adventure"} big={"true"} shadow={"true"}/>
                            <Button text={"Adventure"} big={"true"} shadow={"true"}/>
                            <Button text={"Adventure"} big={"true"} shadow={"true"}/>
                            <Button text={"Adventure"} big={"true"} shadow={"true"}/>
                        </div>
                    </div>
                    <div className="fraction-concrete-category">
                        <div className="category-title">
                            <Icon path={ICONS.path}/>
                            <h1>Romance</h1>
                        </div>
                        <div className="category-content">
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                        </div>
                    </div>
                    <div className="fraction-concrete-category">
                        <div className="category-title">
                            <Icon path={ICONS.path}/>
                            <h1>Adventure</h1>
                        </div>
                        <div className="category-content">
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                            <BookCard />
                        </div>
                    </div>
                </div>
        </>
    )
}

export default mainPage;