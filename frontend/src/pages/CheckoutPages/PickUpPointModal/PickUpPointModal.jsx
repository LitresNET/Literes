import { Map, Placemark, YMaps } from '@pbe/react-yandex-maps';
import { PickUpPointCard } from '../../../components/PickUpPointCard/PickUpPointCard';
import './PickUpPointModal.css'
import { DropDownInputSearch } from '../../../components/DropDownInputSearch/DropDownInputSearch';

export default function PickUpPointModal(props) {
    const {isOpen, onClose, onChoose} = props;

    const pickUpPoints = [
        {id: 1, rating: 0, address: 'ул. Саид-Галеева, д.27', workingHours: '10:00-22:00', coordinates: [55.790200, 49.101159]},
        {id: 2, rating: 4, address: 'ул. Сабан, д.7А', workingHours: '09:00-21:00', coordinates: [55.826680, 49.051338]},
        {id: 3, rating: 3, address: 'ул. Ленинградская, д.32', workingHours: '09:00-23:00', coordinates: [55.866793, 49.088214]},
        {id: 4, rating: 5, address: 'ул. Проспет Победы, д.46', workingHours: '10:00-22:00', coordinates: [55.747277, 49.205968]},
        {id: 5, rating: 4, address: 'ул. Бондаренко, д.34', workingHours: '10:00-22:00', coordinates: [55.815716, 49.105525]},
        {id: 6, rating: 4, address: 'ул. Минская, д.46', workingHours: '09:00-23:00', coordinates: [55.772082, 49.239661]},
        {id: 7, rating: 2, address: 'ул. Фатыха Амирхана, д.21', workingHours: '10:00-22:00', coordinates: [55.823742, 49.131700]},
        {id: 8, rating: 4, address: 'ул. Щапова, д.2/16', workingHours: '00:00-23:59', coordinates: [55.789649, 49.125308]},
        {id: 9, rating: 1, address: 'ул. Нурсултана Назарбанва, д.45А', workingHours: '09:00-23:00', coordinates: [55.769803, 49.134437]},
        {id: 10, rating: 5, address: 'ул. Петра Полушкина, д.6', workingHours: '08:00-21:00', coordinates: [55.785903, 49.216664]},
    ]

    const handleOutsideClick = (event) => {
        if (event.target.classList.contains('modal-shadow')) {
            onClose();
        }
    };
    const handlePointChoiceClick = (point) => {
        onChoose(point)
        onClose();
    };

    if (!isOpen) return null;

    return (
        <>
            <YMaps>
                <div className="modal-shadow" onClick={handleOutsideClick}>
                    <div className="modal-window">
                        <div className="modal-pick-up-points-container">
                            <p>Choose a pick-up point</p>
                            <DropDownInputSearch></DropDownInputSearch>
                            <div className='modal-pick-up-points'>
                                {pickUpPoints.map((item) => (
                                    <div key={item.id}>
                                        <PickUpPointCard rating={item.rating} address={item.address} workingHours={item.workingHours} onClick={() => handlePointChoiceClick(item)}></PickUpPointCard>
                                    </div>
                                ))}
                            </div>
                        </div>
                        <Map className="modal-map" defaultState={{center: [55.796127, 49.106414], zoom: 11}}>
                            {pickUpPoints.map((item, index) => (
                                <Placemark key={index} geometry={item.coordinates} onClick={() => handlePointChoiceClick(item)}/>
                            ))}
                        </Map>
                    </div>
                </div>
            </YMaps>
        </>
    );
}