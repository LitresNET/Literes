import {SubscriptionCard} from "../../../components/SubscriptionCard/SubscriptionCard";
import {useState} from "react";
import {Icon} from "../../../components/UI/Icon/Icon";
import ICONS from "../../../assets/icons.jsx";

function subscriptionPage() {
    const subscriptionCards = [
        <SubscriptionCard key={0} style={{backgroundColor: "red"}}/>,    // убрать все цвета, когда
        <SubscriptionCard key={1} style={{backgroundColor: "orange"}}/>, // начнётся реализация логики
        <SubscriptionCard key={2} style={{backgroundColor: "yellow"}}/>,
        <SubscriptionCard key={3} style={{backgroundColor: "green"}}/>,
        <SubscriptionCard key={4} style={{backgroundColor: "blue"}}/>
    ];

    const [currentBlock, setCurrentBlock] = useState([0, 0, 0]);

    const tripleSubscriptionBlocks = divideArray(subscriptionCards, 3);
    const duoSubscriptionBlocks = divideArray(subscriptionCards, 2);
    const singleSubscriptionBlocks = divideArray(subscriptionCards, 1);

    function divideArray(array, size) {
        let subarray = [];
        for (let i = 0; i < Math.ceil(array.length / size); i++) {
            subarray[i] = array.slice((i * size), (i * size) + size);
        }
        return subarray;
    }

    // скорее всего функцию можно переписать не используя слежение за размером экрана
    function setOtherBlock(n) {
        let w = window.innerWidth;
        let [n0, n1, n2] = currentBlock;

        if (w < 768) {
            if (n0 + n < 0 || n0 + n > Math.floor(subscriptionCards.length - 1)) 
                return 0;
            n0 += n;
        } else if (w < 1200 && w >= 768) {
            if (n1 + n < 0 || n1 + n > Math.floor((subscriptionCards.length - 1) / 2))
                return 0;
            n1 += n;
        } else if (w >= 1200) {
            if (n2 + n < 0 || n2 + n > Math.floor((subscriptionCards.length - 1) / 3))
                return 0;
            n2 += n;
        }
        setCurrentBlock([n0, n1, n2]);
    }

    return (
        <div className={"adaptive"}>
            <h1 className="title">Create A Subscription Plan</h1>
            <div className="subscriptions-carousel">
                <div className="small-subscriptions-block">
                    {singleSubscriptionBlocks[currentBlock[0]]}
                </div>
                <div className="medium-subscriptions-block">
                    {duoSubscriptionBlocks[currentBlock[1]]}
                </div>
                <div className="big-subscriptions-block">
                    {tripleSubscriptionBlocks[currentBlock[2]]}
                </div>
                
            </div>

            <div className="subscription-navigation">
                <Icon onClick={()=> setOtherBlock(-1)} path={ICONS.caret_left} size={"custom"} width={120} />
                <Icon onClick={()=> setOtherBlock(1)} path={ICONS.caret_right} size={"custom"} width={120}/>
            </div>
        </div>
    );
}

export default subscriptionPage;