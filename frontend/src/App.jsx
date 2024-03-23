import {Route, Routes} from "react-router-dom";
import componentsLookup from "./components/componentsLookup.jsx";
import {ROUTES} from './router/router.jsx';


function App() {

    return (
        <>
            <div>
                <Routes>
                    <Route path={ROUTES.components} element={componentsLookup()}></Route>
                </Routes>
            </div>
        </>
    )
}

export default App
