import { hot } from 'react-hot-loader/root';

import React from 'react';
import { HashRouter, Switch, Route } from 'react-router-dom';

import PriceView from '@/Windows/PriceView';
import SettingsView from '@/Windows/SettingsView';

import './style.scss';

function App() {
    return (
        <HashRouter>
            <Switch>
                <Route path="/price-view" component={PriceView} />
                <Route path="/settings" component={SettingsView} />
            </Switch>
        </HashRouter>
    );
}

export default hot(App);
