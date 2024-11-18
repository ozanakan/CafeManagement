import './App.css';
import React, { useState } from 'react'
import store from 'store'
import Loader from './components/loader';
import AppRouter from './router/app-router';

const App = () => {
    const [loaded, setLoaded] = useState(false);
    const onLoaded = () => {
        setLoaded(true)
    }
    const token = store.get('token')
    console.log("loaded", loaded)
    return (
        <>
            <div className="App">
                {loaded ?
                    (
                        <AppRouter />
                    ) :
                    (
                        <Loader onLoaded={onLoaded} token={token} />
                    )}
            </div>
        </>);
}

export default App
