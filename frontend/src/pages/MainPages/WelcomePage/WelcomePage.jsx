import IMAGES from "../../../assets/images.jsx";
import {DropDownInputSearch} from "../../../components/DropDownInputSearch/DropDownInputSearch";
import ICONS from "../../../assets/icons.jsx";
import {Button} from "../../../components/UI/Button/Button.jsx";
import './WelcomePage.css';
import {Icon} from "../../../components/UI/Icon/Icon.jsx";
import {Banner} from "../../../components/UI/Banner/Banner";
import {Cover} from "../../../components/Cover/Cover.jsx";
import {BookCard} from "../../../components/BookCard/BookCard";
import {toast} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import {useEffect, useState} from "react";
import {useGetBookByCategory} from "../../../hooks/useGetBookByCategory.js";
import { Link } from 'react-router-dom'


function mainPage() {
    const [searchField, setSearchField] = useState(null);
    const [popularBooks, setPopularBooks] = useState([]);
    const [romanceBooks, setRomanceBooks] = useState([]);
    const [adventuresBooks, setAdventuresBooks] = useState([]);

    useEffect(() => {
        const fetchPopularBooksData = async () => {
            try {
                const response = await useGetBookByCategory(0, 10, {New: `true`});
                setPopularBooks(response.result);

            } catch (error) {
                toast.error('Popular Books: '+error.message,
                    {toastId: "PopularBooksError"});
            }
        };

        const fetchRomanceBooksData = async () => {
            try {
                const response = await useGetBookByCategory(0, 10, {Category: `RomanceNovel`});
                setRomanceBooks(response.result);
            } catch (error) {
                setErrorToast( () => toast.error('Romance Books: '+error.message,
                    {toastId: "RomanceBooksError"}));
            }
        };
        const fetchAdventureBooksData = async () => {
            try {
                const response = await useGetBookByCategory(0, 10, {Category: `Adventure`});
                setAdventuresBooks(response.result);
            } catch (error) {
                setErrorToast( () => toast.error('Adventure Books: '+error.message,
                    {toastId: "AdventureBooksError"}));
            }
        };

        const fetchWelcomePageData = async () => {
            await fetchPopularBooksData();
            await fetchAdventureBooksData();
            await fetchRomanceBooksData();
        }

        toast.promise(
            fetchWelcomePageData,
            {
                pending: 'Loading'
            }, {toastId: "WelcomePageLoading"});
    }, []);

    return (
        <>
            <div className="main-page">
                <div className="head">
                    <div className="head-content">
                        <h1 className={"font-syne"}>What book you looking for?</h1>
                        <p>Explore our catalog and find your next read.</p>
                        <DropDownInputSearch onChange={e => setSearchField(e.target.value)}/>
                        <div className="head-button">
                            <Link to={`/search?name=${searchField}`}>
                                <Button id="cool-button" text={"Explore"} iconpath={ICONS.binoculars} color={"yellow"}/>
                            </Link>
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
                            {
                                popularBooks?.map(book => <Cover imgPath={book.coverUrl === null ||
                                    book.coverUrl === undefined || book.coverUrl === "" ? IMAGES.default_cover :
                                    book.coverUrl} link={`book/${book.id}`}/>)
                            }
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
                        {
                            romanceBooks?.map(book =>   <BookCard bookId={book.id}/>)
                        }
                    </div>
                </div>
                <div className="fraction-concrete-category">
                    <div className="category-title">
                        <Icon path={ICONS.path}/>
                        <h1>Adventure</h1>
                    </div>
                    <div className="category-content">
                        {
                            adventuresBooks?.map(book =>   <BookCard bookId={book.id}/>)
                        }
                    </div>
                </div>
            </div>
        </>
    )
}

export default mainPage;