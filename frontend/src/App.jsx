import './pages/SubscriptionPages/SubscriptionPage/SubscriptionPage.css';

import {Route, Routes} from "react-router-dom";
import {ROUTES} from './router/router.jsx';
import componentsLookup from "./components/componentsLookup.jsx";
import subscriptionPage from "./pages/SubscriptionPages/SubscriptionPage/SubscriptionPage.jsx";


function App() {

    return (
        <>
            <div>
                <Routes>
                    <Route path={ROUTES.components} element={componentsLookup()}></Route>
                    <Route path={ROUTES.subscriptions_page} element={subscriptionPage()}></Route>
                </Routes>
            </div>
        </>
    )
}

export default App
