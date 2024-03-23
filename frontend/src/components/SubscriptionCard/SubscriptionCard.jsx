// TODO: реализовать компонент карточки тарифного плана.
// При клике, выбранный тариф должен сохраняться в локальном хранилище или кеше

import {Button} from "../UI/Button/Button.jsx";

export function SubscriptionCard(props) {
    const { subscriptionId } = props;

    // логика доставания плана подписки с сервера

    return(
        <>
            <div className="subscription-card">
                <div className="subscription-card-header">
                    <h1>Free</h1>
                </div>
                <div className="subscription-card-content">
                    <ul className="features-list">
                        <li>Key feature 1</li>
                        <li>Key feature 2</li>
                        <li>Key feature 3</li>
                        <li>Key feature 4</li>
                        <li>Key feature 5</li>
                    </ul>
                    <Button text={"$0.00/month"} onClick={() => alert("Заглушка!")} round={"true"}/>
                    <Button text={"Choose"} onClick={() => alert("Заглушка!")} round={"true"} color={"orange"}/>
                </div>
            </div>
        </>
    )
}