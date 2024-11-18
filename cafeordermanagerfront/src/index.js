import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.scss';
import reportWebVitals from './reportWebVitals';
import { Provider } from 'react-redux'
import createSagaMiddleware from 'redux-saga'
import { createStore, applyMiddleware, compose } from 'redux'
import reducers from './redux/reducers'
import { createHashHistory } from 'history'
import { routerMiddleware } from 'connected-react-router'
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import sagas from './redux/sagas'
import i18next from 'i18next';
import { initReactI18next } from 'react-i18next';

const history = createHashHistory()
const sagaMiddleware = createSagaMiddleware()
const routeMiddleware = routerMiddleware(history)
const middlewares = [sagaMiddleware, routeMiddleware]
const store = createStore(reducers(history), compose(applyMiddleware(...middlewares)))



sagaMiddleware.run(sagas)

i18next
  .use(initReactI18next)
  .init({
    resources: {
      tr: {
        translation: require('./locales/tr/translation.json')
      }
    },
    lng: 'tr', // Varsayılan dil
    fallbackLng: 'en', // Düşme durumunda kullanılacak dil
    interpolation: {
      escapeValue: false // React zaten XSS koruması sağlar
    }
  });

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <Provider store={store}>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </Provider>
);

reportWebVitals();
