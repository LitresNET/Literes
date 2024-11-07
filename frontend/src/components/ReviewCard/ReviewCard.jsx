import './ReviewCard.css';

import {Cover} from "../Cover/Cover.jsx";
import {Banner} from "../UI/Banner/Banner";
import IMAGES from "../../assets/images.jsx";
import {Icon} from "../UI/Icon/Icon.jsx";
import ICONS from "../../assets/icons.jsx";
import {useEffect, useState} from "react";
import {CommentCard} from "../CommentCard/CommentCard";
import {axiosToLitres} from "../../hooks/useAxios.js";
import {toast} from "react-toastify";

/// Принимает: <br/>
/// commentId : number - id отзыва (остальное достаёт с сервера)
//TODO: Добавить возможность оценки комментариев
//TODO: Добавить адекватный просмотр подкомментариев
export function ReviewCard(props) {
    const { reviewId, content, rating, createdAt, userId, likes, dislikes, ...rest } = props;

    const [stateDislikes, setStateDislikes] = useState(dislikes);
    const [stateLikes, setStateLikes] = useState(likes);
    const [isOpen, setOpen] = useState(false);

    const [data, setData] = useState(null);
    const [authorData, setAuthorData] = useState(null);

    const [setErrorToast] = useState(null);
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axiosToLitres.get(`/review/${reviewId}`); //TODO:добавить метод на бэк
                setData(response.data);
            } catch (error) {
                setErrorToast( () => toast.error('Review Card: '+error.message,
                    {toastId: "ReviewCardError"}));
            }
        };
        if(reviewId !== null && reviewId !== undefined && reviewId !== 0){
            fetchData();
        }
        const fetchAuthorData = async () => {
            try {
                const response = await axiosToLitres.get(`/user/${userId ?? data?.userId}`);
                setAuthorData(response.data);
            } catch (error) {
                setErrorToast( () => toast.error('Review Card: '+error.message,
                    {toastId: "ReviewCardError"}));
            }
        };
        fetchAuthorData();
    }, []);

    const comments = [1, 2, 3, 4].map((e) =>
        <CommentCard key={e} />
    ); //TODO: Ааа а комменты нельзя с бэка достать вот так вот

    return (
        <>
            <div className="review-card-wrapper" {...rest}>
                <Banner shadow={"true"}>
                    <div className="review-card">
                        <Cover imgPath={authorData?.avatarUrl || IMAGES.avatar_none} size="mini"/>
                        <div className="review-card-info">
                            <div className="review-card-info-row">
                                <h3 className="review-card-info-author">{authorData?.name}</h3>
                                <div className="review-marks-group">
                                    <div onClick={() => setStateLikes(stateLikes + 1)} className="review-mark">
                                        <p>{stateLikes}</p>
                                        <Icon path={ICONS.like}/>
                                    </div>
                                    <div onClick={() => setStateDislikes(stateDislikes + 1)} className="review-mark">
                                        <p>{stateDislikes}</p>
                                        <Icon path={ICONS.dislike}/>
                                    </div>
                                </div>
                            </div>
                            <div className="review-card-info-row">
                                <div className="review-stars">
                                    {Array.from({ length: Math.round(data?.rating ?? rating) }).map(_ =>
                                        <Icon path={ICONS.filled_star} />)}
                                    {Array.from({ length: 5 - Math.round(data?.rating ?? rating)  }).map(_ =>
                                        <Icon path={ICONS.empty_star} />)}
                                </div>
                                <div className="review-date">{(data?.createdAt ?? createdAt)?.replace('T', ' ')}</div>
                            </div>
                            <div className="review-card-info-row">
                                <p className="review-content">
                                    {data?.content ?? content}
                                </p>
                            </div>
                        </div>
                    </div>
                </Banner>
                <div className="review-open-comments">
                    <div className="review-comments-button" onClick={() => setOpen(!isOpen)}>
                        <p>{(isOpen ? "hide" : "view") + " comments"}</p>
                        <Icon path={ICONS.caret_down} size="mini" style={{transform: isOpen ? "rotateZ(-180deg)" : "rotateZ(0)"}}/>
                    </div>
                    <div className={"comments " + (isOpen ? "active" : "")}>
                        {comments}
                        <div className={"comments-add-button"}>
                            <p onClick={() => alert("Заглушка!")}>+ add comment</p>
                        </div>
                    </div>
                </div>

            </div>
        </>
    )
}