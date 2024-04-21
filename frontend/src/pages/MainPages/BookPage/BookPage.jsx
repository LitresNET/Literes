import './BookPage.css'
import ICONS from '../../../assets/icons'
import IMAGES from '../../../assets/images'
import { Cover } from '../../../components/Cover/Cover'
import { Banner } from '../../../components/UI/Banner/Banner'
import { Button } from '../../../components/UI/Button/Button'
import { Icon } from '../../../components/UI/Icon/Icon'
import { BookCard } from '../../../components/BookCard/BookCard'
import { Input } from "../../../components/UI/Input/Input.jsx";

import 'swiper/css'
import { Swiper, SwiperSlide } from 'swiper/react'
import {ReviewCard} from "../../../components/ReviewCard/ReviewCard.jsx";
import {useState} from "react";

export default function BookPage(props) {

    const [value, setValue] = useState('');

    const handleChange = (event) => {
        setValue(event.target.value);
        if (onChange) {
            onChange(event.target.value);
        }
    };


    return (
        <div className="book-page">
            <div className="book-container">
                <Cover imgPath={IMAGES.default_cover} size="big" />
                <div className="book-info">
                    <div className="book-info-title">
                        <h1>Book Name
                            <div className="review-stars">
                                <Icon path={ICONS.filled_star}/>
                                <Icon path={ICONS.filled_star}/>
                                <Icon path={ICONS.filled_star}/>
                                <Icon path={ICONS.empty_star}/>
                                <Icon path={ICONS.empty_star}/>
                            </div>
                        </h1>

                        <div className="book-favorite">
                            <Button iconpath={ICONS.bookmark_simple} onClick={() => (alert("Заглушка!"))}
                                    round={"true"}/>
                        </div>
                    </div>

                    <Banner withshadow="true">
                    <span className="book-banner-name">Author: {"Author name"}</span>
                    </Banner>
                    <Banner withshadow="true">
                        <span className="book-banner-description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam eu turpis molestie, dictum est a, mattis tellus. Sed dignissim, metus nec fringilla accumsan, risus sem sollicitudin lacus, ut interdum tellus elit sed risus. Maecenas eget condimentum velit, sit amet feugiat lectus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.</span>
                    </Banner>

                    <div className="book-price">
                        <span>$30,00</span>
                        <Input type="number"></Input>
                    </div>
                    <div className="book-buttons">
                        <Button
                            round={'true'}
                            color={'yellow'}
                            iconpath={ICONS.shopping_cart}
                        />
                        <Button
                            round={'true'}
                            color={'orange'}
                            text={'Buy Now'}
                        />
                    </div>

                </div>
            </div>
            <div className="books">
                <div className="container-title">
                    <Icon path={ICONS.path}/>
                    <h1>view more</h1>
                </div>
                <Swiper
                    spaceBetween={20}
                    slidesPerView={'auto'}
                    freeMode={true}
                >
                    {Array.from({ length: 10 }).map((_, index) => (
                        <SwiperSlide key={index} style={{ width: 'auto', minWidth: '100px' }}>
                            {/* Здесь задаём минимальную ширину слайда */}
                            <BookCard
                                role={'user'}
                                imgPath={IMAGES.default_cover}
                                description={'maecenas nulla nibh amet non fringilla'}
                            />
                        </SwiperSlide>
                    ))}
                </Swiper>
            </div>
            <div className="reviews">
                <div className="container-title">
                    <Icon path={ICONS.publish}/>
                    <h1>reviews</h1>
                </div>
                <div className="under-reviews-title">
                    <div className="sort-container">
                        <h5 className="font-syne">Sort by</h5>
                        <div className="dropdown-link">
                            <select value={value} onChange={handleChange}>
                                <option value="relevance">relevance</option>
                                <option value="date-asc">date asc</option>
                                <option value="date-desc">date desc</option>
                                <option value="rating">rating</option>
                            </select>
                        </div>
                        <Icon path={ICONS.caret_down} size={'mini'}></Icon>

                    </div>
                    <h5 className="font-syne">+ add review</h5>
                </div>
                <div className="review-cards-wrapper">
                    {Array.from({length: 5}).map((_, index) => (
                        <ReviewCard key={index}>
                            reviewId={'0'}
                        </ReviewCard>
                    ))}
                </div>
            </div>
        </div>
    )
}
