import './UserAccountPage.css'
import ICONS from '../../../assets/icons'
import IMAGES from '../../../assets/images'
import { Cover } from '../../../components/Cover/Cover'
import { Banner } from '../../../components/UI/Banner/Banner'
import { Button } from '../../../components/UI/Button/Button'
import { Icon } from '../../../components/UI/Icon/Icon'
import { Tag } from '../../../components/UI/Tag/Tag'
import { AccountBookCard } from '../../../components/AccountBookCard/AccountBookCard'

import 'swiper/css'
import { Swiper, SwiperSlide } from 'swiper/react'
import { Link } from 'react-router-dom'
import {useEffect, useState} from "react";
import {toast} from "react-toastify";
import {axiosToLitres} from "../../../hooks/useAxios.js";

export default function UserAccountPage() {

    const [userData, setUserData] = useState(null);

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const response = await axiosToLitres.get(`/user/settings`);
                setUserData(response.data);
            } catch (error) {
                toast.error('User Information: '+error.message,
                    {toastId: "UserDataError"});
            }
        };

        toast.promise(
            fetchUserData,
            {
                pending: 'Loading'
            }, {toastId: "UserPageLoading"});
    }, []);

    const subscriptionTag = <Tag status="failure" actionDescription="Deactivated" />;
    //TODO: чет я вообще не понял че должно быть на этой страничке. Откладываю до завтра
    /*
    function renderSubscriptionTag() {
        switch(userData.subscriptionId) {
            case '1':  <Tag status="failure" actionDescription="Deactivated" />
                break;
            case '2':  <Tag status="failure" actionDescription="Deactivated" />
                break;
            case '3':  <Tag status="failure" actionDescription="Deactivated" />
                break;
            case '4':  <Tag status="failure" actionDescription="Deactivated" />
                break;
            case '5':  <Tag status="failure" actionDescription="Deactivated" />
                break;
        }
    }
*/

    return (
        <>
            <div className="wrapper">
                <div className="account-container">
                    <Cover imgPath={userData?.avatarUrl || IMAGES.default_cover} size="big" />
                    <div className="account-info">
                        <div className="account-info-title">
                            <h1>Selected subscription</h1>
                            <Link to="/account/settings" style={{textDecoration: 'none'}}>
                                <div className="account-setting-button">
                                    <Icon path={ICONS.settings} />
                                </div>
                            </Link>
                        </div>
                        <Banner>
                            <span className="account-banner-text">Premium</span>
                        </Banner>
                        {
                            subscriptionTag
                        }

                        <div className="account-balance">
                            <span>BALANCE: </span>
                            <span>${userData?.wallet}</span>
                        </div>
                        <Link to={'/subscription'} style={{textDecoration: 'none'}}>
                            <Button
                                text={'Adventure'}
                                round={'true'}
                                color={'yellow'}
                                iconPath={ICONS.money}
                            />
                        </Link>
                    </div>
                </div>
                <div className="books">
                    <div className="books-title">
                        <Icon path={ICONS.book_open} />
                        <h2>my books</h2>
                    </div>
                    <Swiper
                        spaceBetween={20}
                        slidesPerView={'auto'}
                        freeMode={true}
                    >
                        {userData?.favourites.map(id => (
                            <SwiperSlide style={{ width: 'auto', minWidth: '100px' }}>
                                {/* Здесь задаём минимальную ширину слайда */}
                                <AccountBookCard
                                    key={id}
                                    role={'user'}
                                    bookId={id}
                                />
                            </SwiperSlide>
                        ))}
                    </Swiper>
                </div>
            </div>
        </>
    )
}
