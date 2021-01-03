import React, { useEffect, useState } from 'react';

import { getSettings } from 'common/ipc/ipcHelper';

import './style.scss';

function SettingsView() {
    const [settings, setSettings] = useState<any>(null);

    useEffect(function onInitialize() {
        setSettings(getSettings());
    }, []);

    return <div className="blueprint-view bp3-dark">{settings && <p>{settings['language_UI']}</p>}</div>;
}

export default SettingsView;
