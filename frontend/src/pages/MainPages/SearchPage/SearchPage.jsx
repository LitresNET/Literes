import "./SearchPage.css";
import {DropDownInputSearch} from "../../../components/DropDownInputSearch/DropDownInputSearch";
import {useRef, useState} from "react";
import {DropdownSelect} from "../../../components/UI/DropDownSelect/DropdownSelect";
import {BookCard} from "../../../components/BookCard/BookCard.jsx";
import {Icon} from "../../../components/UI/Icon/Icon";
import ICONS from "../../../assets/icons.jsx";

function searchPage() {
    const categoryFilters = [1, 2, 3, 4].map((n) =>
        <option key={n} value={n}>{n}</option>
    );
    const timeFilters = [1, 2, 3, 4].map(n =>
        <option key={n} value={n}>{n}</option>
    );

    const searchBarTitleDiv = useRef(null);
    const nothingFoundDiv = useRef(null);
    const searchResultsDiv = useRef(null);
    const [searchString, setSearchString] = useState("");
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

    function computeResults(e) {
        e.preventDefault();

        let response = [1, 2, 3, 4]; // запрос на сервер имитация загрузки


        setSearchString(searchedValue);
        searchBarTitleDiv.current.style.visibility = "visible";
        searchBarTitleDiv.current.style.opacity = "1";

        if (response === null) {
            nothingFoundDiv.current.style.visibility = "visible";
            setResults(null);
        }
        else {
            let data = createCards(response);
            searchResultsDiv.current.style.opacity = "0";
            setTimeout(() => setResults(data), 200)
            setTimeout(() => searchResultsDiv.current.style.opacity = "1", 500)
        }

    }

    function createCards(ids) {
        return ids.map((id) =>
            <BookCard key={id} bookId={id} />
        )
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
                        >
                            {categoryFilters}
                        </DropdownSelect>
                        <DropdownSelect
                            selectgroupname={"Filter by"}
                            value={filter}
                            onChange={changeFilter}
                        >
                            {timeFilters}
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