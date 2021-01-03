import { app, clipboard, globalShortcut } from 'electron';
import robotjs from 'robotjs';

import { createTray } from './tray';
import { connect as connectSidekickConsole, connection } from 'common/SidekickConsole';
import { createWindow as createPriceViewWindow, priceViewWindow } from './PriceViewWindow';
import { setupMainHooks } from 'common/ipc/ipcHelper';
import * as ipcEvent from 'common/ipc/ipcEvents';
import { isDevelopment } from 'common/Utils';

robotjs.setKeyboardDelay(0);

app.allowRendererProcessReuse = false;
app.commandLine.appendSwitch('wm-window-animations-disabled');

const installBrowserExtensions = async () => {
    if (isDevelopment) {
        const install = require('electron-devtools-installer');
        const extensions = ['REACT_DEVELOPER_TOOLS', 'REDUX_DEVTOOLS'];

        return Promise.all(extensions.map((name) => install.default(install[name]))).catch(console.log);
    }
    return Promise.resolve();
};

try {
    app.on('ready', () => {
        connectSidekickConsole();
        setupMainHooks();
        installBrowserExtensions();
        createPriceViewWindow();
        createTray();
        registerGlobalHotkeys();
    });

    app.on('will-quit', () => {
        globalShortcut.unregisterAll();
        connection?.close();
    });
} catch (e) {}

function registerGlobalHotkeys() {
    globalShortcut.register('F4', async () => {
        priceViewWindow?.webContents.send('start-loading');
        robotjs.keyTap('C', ['Ctrl']);

        await new Promise((x) => setTimeout(x, 20));

        const itemText = clipboard.readText();

        connection?.send(ipcEvent.PARSE_ITEM, itemText, (error, item) => {
            priceViewWindow?.webContents.send('item-parsed', item);
        });

        if (!priceViewWindow?.isVisible()) {
            setTimeout(() => {
                priceViewWindow?.webContents.send('show');
            }, 100);
        }
    });
}
