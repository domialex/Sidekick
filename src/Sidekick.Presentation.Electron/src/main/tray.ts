import { app, Menu, MenuItem, Tray } from 'electron';
import path from 'path';

import { createWindow as createSettingsWindow } from './settingsWindow';

let tray;

declare const __static: string;

export function createTray() {
    tray = new Tray(path.join(__static, 'icon.png'));

    const items = [
        new MenuItem({
            label: 'Settings',
            type: 'normal',
            click: createSettingsWindow,
        }),
        new MenuItem({
            label: 'Exit',
            type: 'normal',
            click: () => app.quit(),
        }),
    ];

    const menu = Menu.buildFromTemplate(items);
    tray.setToolTip(`Sidekick: ${app.getVersion()}`);
    tray.setContextMenu(menu);
}
