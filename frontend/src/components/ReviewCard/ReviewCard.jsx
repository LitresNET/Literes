import './ReviewCard.css';

import {Cover} from "../Cover/Cover.jsx";
import {Banner} from "../UI/Banner/Banner";
import IMAGES from "../../assets/images.jsx";
import {Icon} from "../UI/Icon/Icon.jsx";
import ICONS from "../../assets/icons.jsx";
import {useState} from "react";
import {CommentCard} from "../CommentCard/CommentCard";

/// Принимает: <br/>
/// commentId : number - id отзыва (остальное достаёт с сервера)
export function ReviewCard(props) {
    const { reviewId } = props;

    const [dislikes, setDislike] = useState(149);
    const [likes, setLike] = useState(89);
    const [isOpen, setOpen] = useState(false);

    const comments = [1, 2, 3, 4].map((e) =>
        <CommentCard key={e} />
    );


    // логика доставания обзора и комментов с сервера

    return (
        <>
            <div className="review-card-wrapper">
                <Banner withshadow={"true"}>
                    <div className="review-card">
                        <Cover imgPath={IMAGES.avatar_none} size="mini"/>
                        <div className="review-card-info">
                            <div className="review-card-info-row">
                                <h3 className="review-card-info-author">Ivan Ivanov</h3>
                                <div className="review-marks-group">
                                    <div onClick={() => setLike(likes + 1)} className="review-mark">
                                        <p>{likes}</p>
                                        <Icon path={ICONS.like}/>
                                    </div>
                                    <div onClick={() => setDislike(dislikes + 1)} className="review-mark">
                                        <p>{dislikes}</p>
                                        <Icon path={ICONS.dislike}/>
                                    </div>
                                </div>
                            </div>
                            <div className="review-card-info-row">
                                <div className="review-stars">
                                    <Icon path={ICONS.filled_star}/>
                                    <Icon path={ICONS.filled_star}/>
                                    <Icon path={ICONS.filled_star}/>
                                    <Icon path={ICONS.empty_star}/>
                                    <Icon path={ICONS.empty_star}/>
                                </div>
                                <div className="review-date">21 Mar, 2024</div>
                            </div>
                            <div className="review-card-info-row">
                                <p className="review-content">
                                    Lorem ipsum dolor sit amet,
                                    consectetur adipiscing elit. Etiam eu turpis molestie,
                                    dictum est a, mattis tellus. Sed dignissim, metus nec
                                    fringilla accumsan, risus sem sollicitudin lacus, ut interdum
                                    tellus elit sed risus. Maecenas eget condimentum velit, sit
                                    amet feugiat lectus. Class aptent taciti sociosqu ad litora
                                    torquent per conubia nostra, per inceptos himenaeos.
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