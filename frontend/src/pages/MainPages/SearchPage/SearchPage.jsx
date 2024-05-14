import "./SearchPage.css";
import {DropDownInputSearch} from "../../../components/DropDownInputSearch/DropDownInputSearch";
import {useRef, useState} from "react";
import {DropdownSelect} from "../../../components/UI/DropDownSelect/DropdownSelect";
import {BookCard} from "../../../components/BookCard/BookCard.jsx";
import {Icon} from "../../../components/UI/Icon/Icon";
import ICONS from "../../../assets/icons.jsx";
import { useGetBookByCategory } from '../../../hooks/useGetBookByCategory.js'
import { useLocation } from 'react-router-dom'

function searchPage() {
    const location = useLocation();
    const searchParams = new URLSearchParams(location.search);
    const paramValue = searchParams.get('name');

    const searchBarTitleDiv = useRef(null);
    const nothingFoundDiv = useRef(null);
    const searchResultsDiv = useRef(null);
    const [searchString, setSearchString] = useState(paramValue);
    const [searchedValue, setSearchedValue] = useState(searchString);
    const [results, setResults] = useState(undefined);

    const [filter, setFilter] = useState();
    const [category, setCategory] = useState();

    const changeFilter = (selectedFilter) => {
        setFilter(selectedFilter)
    }

    const changeCategory = (selectedCategory) => {
        setCategory(selectedCategory)
    }

    async function computeResults(e) {
        e.preventDefault();

        let params = {
            SearchedValue: undefined,
            Category: undefined,
            New: undefined,
            HighRated: undefined
        }

        if (searchedValue !== null) params.SearchedValue = searchedValue;
        if (category !== null) params.Category = category;
        if (filter === "New") params.New = true;
        if (filter === "HighRated") params.HighRated = true;

        let response = await useGetBookByCategory(0, 20, params);

        setSearchString(searchedValue);
        searchBarTitleDiv.current.style.visibility = "visible";
        searchBarTitleDiv.current.style.opacity = "1";

        if (response.result.length <= 0) {
            nothingFoundDiv.current.style.visibility = "visible";
            setResults(null);
        }
        else {
            let data = createCards(response.result);
            searchResultsDiv.current.style.opacity = "0";
            setTimeout(() => setResults(data), 200)
            setTimeout(() => searchResultsDiv.current.style.opacity = "1", 500)
        }

    }

    function createCards(books) {
        let result = books.map((book) =>
            <BookCard key={book.id} bookId={book.id} />
        );
        return result;
    }

    return (
        <div className="search-page" >
            <form onSubmit={e => computeResults(e)}>
                <DropDownInputSearch value={searchedValue} onChange={e => setSearchedValue(e.target.value)}/>
            </form>
            <div className="search">
                <div className="search-bar">
                    <div className="search-bar-title" ref={searchBarTitleDiv}>
                        <h3>Results for: </h3>
                        <span className="search-string">{searchString}</span>
                    </div>
                    <div className="filters">
                        <DropdownSelect
                            selectgroupname={"Category"}
                            value={category}
                            onChange={changeCategory}
                            style={{overflowX: "scroll", maxWidth: "200px"}}
                        >
                            <option value={null}>Category</option>
                            <option value={"Public"}>Public</option>
                            <option value={"Action"}>Action</option>
                            <option value={"Adventure"}>Adventure</option>
                            <option value={"Autobiography"}>Autobiography</option>
                            <option value={"Biography"}>Biography</option>
                            <option value={"Classic"}>Classic</option>
                            <option value={"Cookbook"}>Cookbook</option>
                            <option value={"Detective"}>Detective</option>
                            <option value={"Dictionary"}>Dictionary</option>
                            <option value={"Encyclopedia"}>Encyclopedia</option>
                            <option value={"Fairytale"}>Fairytale</option>
                            <option value={"Fantasy"}>Fantasy</option>
                            <option value={"Folklore"}>Folklore</option>
                            <option value={"Graphic"}>Graphic</option>
                            <option value={"Historical"}>Historical</option>
                            <option value={"Horror"}>Horror</option>
                            <option value={"RomanceNovel"}>RomanceNovel</option>
                            <option value={"ScienceFiction"}>ScienceFiction</option>
                            <option value={"Textbook"}>Textbook</option>
                        </DropdownSelect>
                        <DropdownSelect
                            selectgroupname={"Filter by"}
                            value={filter}
                            onChange={changeFilter}
                        >
                            <option value={null}>Filter by</option>
                            <option value={"New"}>By date</option>
                            <option value={"HighRated"}>By rating</option>
                        </DropdownSelect>
                    </div>
                </div>
                <div className="search-results-wrapper">
                    <div className="search-nothing-found" ref={nothingFoundDiv}>
                        <Icon path={ICONS.book_open} size={"custom"} width={"150"}/>
                        <p>Nothing was found :\</p>
                    </div>
                    <div className="search-results" ref={searchResultsDiv}>
                        {results}
                    </div>
                </div>
            </div>
        </div>
    )
}

export default searchPage;