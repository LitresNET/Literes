import ICONS from '../../assets/icons';
import { Icon } from '../../components/UI/Icon/Icon';
import { IconButton } from '../../components/UI/Icon/IconButton/IconButton';
import './MainLayout.css';
import { Link, Outlet } from "react-router-dom";
import ShoppingCartSidePanel from "./../ShoppingCartSidePanel/ShoppingCartSidePanel";
import {useState} from 'react';
import authStore from "../../store/store.js";
import {observer} from "mobx-react-lite";

const MainLayout = observer(() => {
    const [isSidePanelOpen, setPanelOpen] = useState(false);

    const handleLogout = () => {
        authStore.logout();
    };
    const links = {
        linked_in: 'https://docs.google.com/spreadsheets/d/1KwgJcmW-W2pFGUNEknmfJ6GnDMTb-gJrZUAjYA1jEAQ/edit#gid=2048177681',
        github: 'https://github.com/LitresNET/Literes',
        figma: 'https://www.figma.com/file/JgLXVgSYJJJ0rIgujBA6IM/Litres-BookStore?type=design&node-id=63%3A1581&mode=design&t=1433fERh9eWZJcq5-1'
    }

    return (
        <>
        <ShoppingCartSidePanel isOpen={isSidePanelOpen} handleClose={() => setPanelOpen(false)}/>
            <div className="page-container">
                <header className='page-header'>
                    <Link to="/" style={{textDecoration: 'none'}}>
                        <div className="page-logo">
                            <Icon path={ICONS.book_open}/>
                            <h3>bookstore</h3>
                        </div>
                    </Link>
                    <div className="page-options">
                        { authStore.isAuthenticated ? (
                            <Link to="/account" style={{textDecoration: 'none'}}>
                                <Icon path={ICONS.account}/>
                            </Link> ) : null }
                        {   authStore.isAuthenticated ? (
                            <Link to="#" onClick={handleLogout} style={{textDecoration: 'none'}}>
                                <Icon path={ICONS.sign_out}/>
                            </Link> ) : (
                            <Link to="/signin" style={{textDecoration: 'none'}}>
                                <Icon path={ICONS.sign_in}/>
                            </Link>
                        ) }
                        <div onClick={() => setPanelOpen(true)} style={{ cursor: 'pointer' }}>
                            <Icon path={ICONS.shopping_cart}/>
                        </div>
                    </div>
                </header>
                <div className="page-content">
                    <Outlet/>
                </div>
                <footer className='page-footer'>
                    <hr className='page-footer-separator'/>
                    <div className="page-footer-content">
                        <p>Litres Project</p>
                        <div className="page-footer-links">
                            <IconButton href={links.linked_in} path={ICONS.linked_in_logo}></IconButton>
                            <IconButton href={links.github} path={ICONS.github_logo}></IconButton>
                            <IconButton href={links.figma} path={ICONS.figma_logo}></IconButton>
                        </div>
                        <span className='team-name-wrapper'>by <span className='team-name'>Стажеры с прямыми руками</span> team</span>
                    </div>
                </footer>
            </div>
        </>
    )
})

export default MainLayout