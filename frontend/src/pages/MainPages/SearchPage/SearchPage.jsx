// TODO: сверстать страницу поиска книги
import "./SearchPage.css";
import {DropDownInputSearch} from "../../../components/DropDownInputSearch/DropDownInputSearch";
import {useState} from "react";
import {DropdownSelect} from "../../../components/UI/DropDownSelect/DropdownSelect";

function searchPage() {
    const categoryFilters = [1, 2, 3, 4].map((n) =>
        <option key={n} value={n}>n</option>
    );
    const timeFilters = [1, 2, 3, 4].map(n =>
        <option key={n} value={n}>n</option>
    );



    const [searchString, setSearchString] = useState("");
    const [results, setResults] = useState(null)

    return (
        <div className="adaptive">
            <DropDownInputSearch value={searchString}>
                <p>Пиво!</p>
            </DropDownInputSearch>
            <div className="search">
                <h3>Results "<p className="search-string">{searchString}</p>"</h3>
                <div className="filters">
                    <DropdownSelect selectgroupname={"Category"} className="filter-category">
                        {categoryFilters}
                    </DropdownSelect>
                    <DropdownSelect selectgroupname={"Filter by"} className="filter-time">
                        {timeFilters}
                    </DropdownSelect>
                </div>
            </div>
            <div className="search-results">
                {results}
            </div>
        </div>
    )
}

export default searchPage;