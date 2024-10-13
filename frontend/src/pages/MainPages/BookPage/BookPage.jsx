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
import {useEffect, useState} from "react";
import {useGetBookByCategory} from "../../../hooks/useGetBookByCategory.js";
import {toast} from "react-toastify";
import {axiosToLitres} from "../../../hooks/useAxios.js";
import {useParams} from "react-router-dom";
import {addBookToFavourites} from "../../../features/addBookToFavourites.js";

//TODO: Нужен рефактор (как в принципе и другим страницам)
export default function BookPage() {

    const [value, setValue] = useState('');
    const [bookData, setBookData] = useState(null);
    const [sameGenreBooks, setSameGenreBooks] = useState([])
    const [reviews, setReviews] = useState([])
    const { id } = useParams();

    const [setErrorToast] = useState(null);

    useEffect(() => {
        const fetchBookData = async () => {
            try {
                const response = await axiosToLitres.get(`/book/${id}`);
                setBookData(response.data);
            } catch (error) {
                setErrorToast( () => toast.error('Book Information: '+error.message,
                    {toastId: "BookDataError"}));
            }
        };
        //TODO: Починить, он работает как говно
        const fetchSameGenreBooksData = async () => {
            try {
                const response = await useGetBookByCategory(0, 10, {Category:
                        bookData?.bookGenres[Math.floor(Math.random() * bookData?.bookGenres?.length)]}); //рандомный жанр
                setSameGenreBooks(response.result);
            } catch (error) {
                setErrorToast( () => toast.error('Same Genre Books: '+error.message,
                    {toastId: "SameGenreBooksError"}));
            }
        };
        //TODO: с бэка приходит по 15 комментариев. Реализовать подгрузку следующих через параметр n
        const fetchReviews = async (n) => {
            try {
                const response = await axiosToLitres.get(`/review/list?bookId=${id}&n=${n ?? 0}`);
                setReviews(response.data);
            } catch (error) {
                setErrorToast( () => toast.error('Reviews: '+error.message,
                    {toastId: "ReviewsError"}));
            }

        }
        const fetchBookPageData = async () => {
            await fetchBookData();
            await fetchSameGenreBooksData();
            await fetchReviews();
        }
        toast.promise(
            fetchBookPageData,
            {
                pending: 'Loading'
            }, {toastId: "BookPageLoading"});
    }, [id]);

    const handleChange = (event) => {
        setValue(event.target.value);
        if (onChange) {
            onChange(event.target.value);
        }
    };


    return (
        <div className="book-page">
            <div className="book-container">
                <Cover imgPath={!bookData?.coverUrl ?
                    IMAGES.default_cover : bookData.coverUrl} size="big" />
                <div className="book-info">
                    <div className="book-info-title">
                        <h1>{bookData?.name}
                            <div className="review-stars">
                                {Array.from({ length: Math.round(bookData?.rating) }).map(_ =>
                                    <Icon path={ICONS.filled_star} />)}
                                {Array.from({ length: 5 - Math.round(bookData?.rating) }).map(_ =>
                                    <Icon path={ICONS.empty_star} />)}
                            </div>
                        </h1>

                        <div className="book-favourite">
                            <Button iconpath={ICONS.bookmark_simple} onClick={async () =>
                                await addBookToFavourites(id)} round={"true"}/>
                        </div>
                    </div>

                    <Banner withshadow="true">
                    <span className="book-banner-name">Author: {bookData?.author}</span>
                    </Banner>
                    <Banner withshadow="true">
                        <span className="book-banner-description">{bookData?.description}</span>
                    </Banner>

                    <div className="book-price">
                        <span>{bookData?.price}$</span>
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
                    {sameGenreBooks?.map(book => (
                        <SwiperSlide style={{ width: 'auto', minWidth: '100px' }}>
                            {/* Здесь задаём минимальную ширину слайда */}
                            <BookCard bookId={book.id}/>
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
                    {reviews?.map(review => (
                        <ReviewCard
                            content={review.content}
                            rating={review.rating}
                            createdAt={review.createdAt}
                            userId={review.userId}
                            likes={review.likes}
                            dislikes={review.dislikes}
                            //reviewId={review.id}
                        />
                    ))}
                </div>
            </div>
        </div>
    )
}
