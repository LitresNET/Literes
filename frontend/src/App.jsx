import {Route, Routes} from "react-router-dom";
import componentsLookup from "./components/componentsLookup.jsx";

function App() {

    return (
        <>
            <div>
                <Routes>
                    <Route path='/components' element={componentsLookup()}></Route>
                </Routes>
            </div>
        </>
    )
}

export default App
