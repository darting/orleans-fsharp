import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { createStore } from 'redux';
import { initializeIcons } from '@uifabric/icons';
import { App } from './App';
import { counterReducer } from './Reducers/CounterReducer';

function start() {
    initializeIcons();

    const store = createStore(counterReducer, { counter: 0 });

    ReactDOM.render(
        <Provider store={store}>
            <App />
        </Provider>,
        document.getElementById('root') as HTMLElement
    );
}

start();
