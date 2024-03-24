import { useState } from 'react';
import { Checkbox } from '../../../components/UI/Checkbox/Checkbox';
import './CustomSubscriptionPage.css'
import { Banner } from '../../../components/UI/Banner/Banner';
import { Button } from '../../../components/UI/Button/Button';

export default function CustomSubscriptionPage() {
    const loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam eu turpis molestie, dictum est a, mattis tellus. Sed dignissim, metus nec fringilla accumsan, risus sem sollicitudin lacus, ut interdum tellus elit sed risus. Maecenas eget condimentum velit, sit amet feugiat lectus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos."
    const features = [
        {id: 1, name: 'Key feature 1', description: loremIpsum, price: 30},
        {id: 2, name: 'Key feature 2', description: loremIpsum, price: 30},
        {id: 3, name: 'Key feature 3', description: loremIpsum, price: 30},
        {id: 4, name: 'Key feature 4', description: loremIpsum, price: 30},
        {id: 5, name: 'Key feature 5', description: loremIpsum, price: 30},
        {id: 6, name: 'Key feature 6', description: loremIpsum, price: 30},
        {id: 7, name: 'Key feature 7', description: loremIpsum, price: 30},
        {id: 8, name: 'Key feature 8', description: loremIpsum, price: 30},
    ]
    const [currentFeature, setCurrent] = useState(features[0]);
    const [selectedFeatures, setSelected] = useState([]);

    const handleFeatureChoose = (feature) => {
        const isFeatureSelected = selectedFeatures.some(selectedFeature => selectedFeature.id === feature.id);
        if (isFeatureSelected) {
            const updatedSelectedFeatures = selectedFeatures.filter(selectedFeature => selectedFeature.id !== feature.id);
            setSelected(updatedSelectedFeatures);
            setCurrent(selectedFeatures[0])
        } else {
            setSelected([...selectedFeatures, feature]);
            setCurrent(feature)
        }
        console.log(selectedFeatures)
    }

    return (
        <>
            <div className="page-wrapper">
                <h1>Create your custom subscription</h1>
                <div className="page-features-wrapper">
                    <div className="page-features">
                        {features.map((item) => (
                            <div key={item.id} onClick={() => handleFeatureChoose(item)}>
                                {/* TODO: чекбокс никак не помечается, когда он выбран */}
                                <Checkbox>{item.name}</Checkbox>
                            </div>
                        ))}
                    </div>
                    <div className="page-description">
                        <Banner withshadow={true}>
                            <span>{currentFeature.name}</span>
                        </Banner>
                        <Banner withshadow={true}>
                            <p>{currentFeature.description}</p>
                        </Banner>
                        {/* TODO: Разобраться, почему кастомные стили на Banner сбрасывают базовые */}
                        <Banner>
                            <div className="cheque-wrapper">
                                <div className="cheque-total">
                                    <span>Subtotal:</span>
                                    <Button color="yellow" text={`$${selectedFeatures.reduce((sum, feature) => sum + feature.price, 0)}`}></Button>
                                </div>
                                <Button color="orange" text={"Pay with stripe"}></Button>
                            </div>
                        </Banner>
                    </div>
                </div>
            </div>
        </>
    )
}