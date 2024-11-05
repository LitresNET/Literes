/// Принимает: <br/>
/// commentId : number - id отзыва (остальное достаёт с сервера)
import {Banner} from "../UI/Banner/Banner";

export function CommentCard(props) {
    const { commentId } = props

    // Доставание данных с сервера о коммнентарии...

    return (
        <>
            <div className="comment-wrapper" {...props}>
                <Banner shadow={"true"}>
                    <div className="comment">
                        <h3>Petrov Petya</h3>
                        <h4>24 Mar, 2024</h4>
                        <p>
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                            Etiam eu turpis molestie, dictum est a, mattis tellus.
                            Sed dignissim, metus nec fringilla accumsan, risus sem
                            sollicitudin lacus, ut interdum tellus elit sed risus.
                            Maecenas eget condimentum velit,
                        </p>
                    </div>
                </Banner>
            </div>
        </>
    )
}