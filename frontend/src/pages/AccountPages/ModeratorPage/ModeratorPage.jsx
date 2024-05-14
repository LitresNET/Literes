import './ModeratorPage.css';
import { useState } from 'react'
import { DropdownSelect } from '../../../components/UI/DropDownSelect/DropdownSelect'
import { Button } from '../../../components/UI/Button/Button'
import {axiosToLitres} from "../../../hooks/useAxios.js";


export default function ModeratorPage() {
    const requestData = [
        {
            id: 1,
            type: 'Credentials',
            publisher: 'Издательство А',
            Credentials: 'http://example.com/cred',
            Download: 'http://example.com/cred'
        },
        {
            id: 2,
            type: 'Book',
            publisher: 'Издательство Б',
            link: 'http://example.com/book',
            additional: 'http://example.com/cred'
        },
        {
            id: 3,
            type: 'Article',
            publisher: 'Издательство В',
            link: 'http://example.com/article',
            additional: 'http://example.com/cred' 
        },
        {
            id: 4,
            type: 'Journal',
            publisher: 'Издательство Г',
            link: 'http://example.com/journal',
            additional: 'http://example.com/cred'
        },
        {
            id: 5,
            type: 'Report',
            publisher: 'Издательство Д',
            link: 'http://example.com/report',
            additional: 'http://example.com/cred'
        },
        {
            id: 6,
            type: 'Thesis',
            publisher: 'Издательство Е',
            link: 'http://example.com/thesis',
            additional: 'http://example.com/cred'
        },
        {
            id: 7,
            type: 'Newsletter',
            publisher: 'Издательство Ж',
            link: 'http://example.com/newsletter',
            additional: 'http://example.com/cred'
        }
    ]

    const [filter, setFilter] = useState('Все')
    const [filteredData, setFilteredData] = useState(requestData)

    const handleFilterChange = (selectedFilter) => {
        setFilter(selectedFilter)

        if (selectedFilter === 'Все') {
            setFilteredData(requestData)
        } else {
            setFilteredData(requestData.filter((item) => item.type === selectedFilter))
        }
    }

    const sendSolution = async (requestId, isAccepted) => {
        try {
            const response = await axiosToLitres.post(`api/request/${requestId}?isAccepted=${isAccepted}`);
        } catch (e) {
            return {result: null, error: e.response.data.errors[0].description};
        }
    }

    return (
        <>
             <div className="moderator-page-title-container">
                <div className="moderator-page-title">
                  <h1>all requests</h1>
                </div>
                <div className="moderator-page-info-container">
                    <span>You’re signed in as</span>
                    <span> super-moderator-123</span>
                </div>
            </div>
            <div className="moderator-page-table-container">
                <DropdownSelect
                    className="dropdown-wrapper"
                    value={filter}
                    onChange={handleFilterChange}
                >
                    <p value="Все">Все</p>
                    <p value="Credentials">Credentials</p>
                    <p value="Book">Book</p>
                </DropdownSelect>
                <table className='moderator-page-table'>
                    <thead>
                        <tr>
                            <th>Type</th>
                            <th>Publisher</th>
                            <th>Credentials</th>
                            <th>Book</th>
                            <th></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredData.map((item) => (
                            <tr key={item.id}>
                                <td>{item.type}</td>
                                <td>{item.publisher}</td>
                                <td>
                                    <a href={item.Credentials} target="_blank" rel="noopener noreferrer">
                                      Download
                                    </a>
                                </td>
                                <td>
                                    <a href={item.Download} target="_blank" rel="noopener noreferrer">
                                      Download
                                    </a>
                                </td>
                                <td>
                                    <Button text={'Accept'} round={'true'} color={'yellow'}
                                            onClick={() => sendSolution(item.id, true)} />
                                </td>
                                <td>
                                    <Button text={'Decline'} round={'true'} color={'orange'}
                                            onClick={() => sendSolution(item.id, false)}/>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </>
    )
}
