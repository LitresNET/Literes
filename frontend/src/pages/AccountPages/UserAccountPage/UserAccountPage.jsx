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
import {Link} from 'react-router-dom'
import {Input} from "../../../components/UI/Input/Input";

export default function UserAccountPage() {

    async function refillRequest() {
        let refillField = document.getElementById("refill-input");
        let amount = refillField.value;

        window.open(`http://localhost:5032/api/order/refill?amount=${amount}`);
    }

    return (
        <>
            <div className="wrapper">
                <div className="account-container">
                    <Cover imgPath={IMAGES.default_cover} size="big" />
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
                        <Tag status="failure" actiondescription="Deactivated" />

                        <div className="account-balance">
                            <span>Current balance: </span>
                            <span>$30,00</span>
                        </div>
                        <div className="account-refill">
                            <Input type={"number"} id={"refill-input"}/>
                            <Button
                                text={'Refill'}
                                round={'true'}
                                color={'yellow'}
                                iconpath={ICONS.money}
                                onClick={refillRequest}
                            />
                        </div>
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
                        {Array.from({ length: 10 }).map((_, index) => (
                            <SwiperSlide key={index} style={{ width: 'auto', minWidth: '100px' }}>
                                {/* Здесь задаём минимальную ширину слайда */}
                                <AccountBookCard
                                    role={'user'}
                                    imgPath={IMAGES.default_cover}
                                    description={'maecenas nulla nibh amet non fringilla'}
                                />
                            </SwiperSlide>
                        ))}
                    </Swiper>
                </div>
            </div>
        </>
    )
}
