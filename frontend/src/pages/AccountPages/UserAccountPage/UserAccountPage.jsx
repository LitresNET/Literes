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

export default function UserAccountPage() {
    return (
        <>
            <div className="wrapper">
                <div className="accaunt-container">
                    <Cover imgPath={IMAGES.default_cover} size="big" />
                    <div className="accaunt-info">
                        <div className="accaunt-info-title">
                            <h1>Selected subscription</h1>
                            <div className="accaunt-setting-button">
                                <Icon path={ICONS.settings} />
                            </div>
                        </div>
                        <Banner>
                            <span className="accaunt-banner-text">Premium</span>
                        </Banner>
                        <Tag status="failure" actiondescription="Deactivated" />

                        <div className="accaunt-balance">
                            <span>BALANCE: </span>
                            <span>$30,00</span>
                        </div>
                        <Button
                            text={'Adventure'}
                            round={'true'}
                            color={'yellow'}
                            iconpath={ICONS.money}
                        />
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
