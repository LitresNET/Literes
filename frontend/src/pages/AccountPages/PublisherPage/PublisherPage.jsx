import './PublisherPage.css'
import ICONS from '../../../assets/icons'
import IMAGES from '../../../assets/images'
import { Cover } from '../../../components/Cover/Cover'
import { Banner } from '../../../components/UI/Banner/Banner'
import { Icon } from '../../../components/UI/Icon/Icon'
import { AccountBookCard } from '../../../components/AccountBookCard/AccountBookCard'

import 'swiper/css'
import { Swiper, SwiperSlide } from 'swiper/react'

export default function PublisherPage() {
    return (
        <>
            <div className="wrapper">
                <div className="publisher-container">
                    <Cover imgPath={IMAGES.default_cover} size="big" />
                    <div className="publisher-info">
                        <h1>publishing house</h1>
                        <Banner>
                            <span className="publisher-banner-text">Publisher A</span>
                        </Banner>
                        <div className="publisher-balance">
                            <span>BALANCE: </span>
                            <span>$30,00</span>
                        </div>
                    </div>
                </div>
                <div className="statistics-container">
                    <h2 className="statistics-title">statistics</h2>
                    <Banner>
                        <div className="statistics-wrapper">
                            <div>
                                <div className="statistics-item">
                                    <div className="statistics-item-icon">
                                        <Icon path={ICONS.money} size={'custom'} width={'45'} />
                                    </div>
                                    <span>
                                        <span>1000 </span>
                                        <span>paper copies sold</span>
                                    </span>
                                </div>
                                <div className="statistics-item">
                                    <div className="statistics-item-icon">
                                        <Icon path={ICONS.publish} size={'custom'} width={'45'} />
                                    </div>
                                    <span>
                                        <span>100 </span>
                                        <span>books published </span>
                                    </span>
                                </div>
                                <div className="statistics-item">
                                    <div className="statistics-item-icon">
                                        <Icon path={ICONS.book_open} size={'custom'} width={'45'} />
                                    </div>
                                    <span>
                                        <span>300.000 </span>
                                        <span>people are reading</span>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </Banner>
                </div>
                <div className="books">
                    <div className="books-title">
                        <Icon path={ICONS.book_open} />
                        <h2>my books</h2>
                    </div>
                    <Swiper spaceBetween={20} slidesPerView={'auto'} freeMode={true}>
                        {Array.from({ length: 10 }).map((_, index) => (
                            <SwiperSlide key={index} style={{ width: 'auto', minWidth: '100px' }}>
                                <AccountBookCard
                                    role={'publisher'}
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
