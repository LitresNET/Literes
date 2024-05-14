import IMAGES from "../../../../assets/images.jsx";
import {Cover} from "../../../../components/Cover/Cover.jsx";
import {Input} from "../../../../components/UI/Input/Input.jsx";
import {Button} from "../../../../components/UI/Button/Button.jsx";
import './BookEditPage.css';
import {Icon} from "../../../../components/UI/Icon/Icon.jsx";
import ICONS from "../../../../assets/icons.jsx";

export default function BookEditPage() {

    return (
        <>
            <div className={'wrapper-edit-container'}>
                <div className={'book-edit-container'}>
                    <h1>BOOK SETTINGS</h1>
                    <div className={'book-edit-picture'}>
                        <Cover imgPath={IMAGES.avatar_none} size={"big"}/>
                        <button className={"book-edit-button-upload"}><Icon path={ICONS.download} size={"default"}/></button>
                        <button className={"book-edit-button-delete"}><Icon path={ICONS.trash} size={"default"}/></button>
                    </div>
                        <form className={'book-edit-form'}>
                            <h1>BOOK SETTINGS</h1>
                            <div className={'book-edit-label-input'}>
                                <label>Title</label>
                                <Input placeholder="Title" type="text"/>
                            </div>
                            <div className={'book-edit-label-input'}>
                                <label>Author</label>
                                <Input placeholder="Author's name" type="text"/>
                            </div>
                            <div className={'book-edit-label-input'}>
                                <label>Description</label>
                                <textarea className={'book-edit-textarea'} placeholder={'Description'}></textarea>
                            </div>
                            <div className={'book-edit-label-input-price'}>
                                <label>Price</label>
                                <Input placeholder="Price" type="text"/>
                            </div>
                            <Button text={"Save"} onClick={() => alert("TODO")} round={"true"} color={"orange"}/>
                            <a className={'book-edit-delete'}>Delete book</a>
                        </form>
                </div>
            </div>
        </>
    )
}