// TODO: сверстать layout, включающий в себя header, footer и sidebar (выезжающую панель с корзиной)
import ICONS from '../../assets/icons';
import { Icon } from '../../components/UI/Icon/Icon';
import { IconButton } from '../../components/UI/Icon/IconButton/IconButton';
import './MainLayout.css';
import { Outlet } from "react-router-dom";

export default function MainLayout() {
    const links = {
        linked_in: 'https://docs.google.com/spreadsheets/d/1KwgJcmW-W2pFGUNEknmfJ6GnDMTb-gJrZUAjYA1jEAQ/edit#gid=2048177681',
        github: 'https://github.com/LitresNET/Literes',
        figma: 'https://www.figma.com/file/JgLXVgSYJJJ0rIgujBA6IM/Litres-BookStore?type=design&node-id=63%3A1581&mode=design&t=1433fERh9eWZJcq5-1'
    }

    return (
        <>
            <div className="page-container">
                <header className='page-header'>
                    <div className="page-logo">
                        <Icon path={ICONS.book_open}/>
                        <h3>bookstore</h3>
                    </div>
                    <div className="page-options">
                        <Icon path={ICONS.account}/>
                        <Icon path={ICONS.sign_in}/>
                        <Icon path={ICONS.shopping_cart}/>
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
}