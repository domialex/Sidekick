import { BrowserWindow, globalShortcut } from 'electron';

const windowWidth = 420;
const windowHeight = 320;

export let priceViewWindow: BrowserWindow | undefined;

export function createWindow() {
    if (priceViewWindow !== undefined) {
        return;
    }

    priceViewWindow = new BrowserWindow({
        transparent: true,
        fullscreenable: false,
        skipTaskbar: true,
        focusable: true,
        webPreferences: {
            nodeIntegration: true,
            enableRemoteModule: true,
            webSecurity: false,
        },
        frame: false,
        minimizable: false,
        maximizable: false,
        width: windowWidth,
        height: windowHeight,
        minWidth: windowWidth,
        minHeight: windowHeight,
        show: false,
    });
    priceViewWindow.removeMenu();
    priceViewWindow.setAlwaysOnTop(true, 'screen-saver');
    priceViewWindow.setVisibleOnAllWorkspaces(true);

    priceViewWindow.on('show', () => {
        globalShortcut.register('F3', () => {
            priceViewWindow!.webContents.send('close');
        });
    });

    priceViewWindow.on('hide', () => {
        globalShortcut.unregister('F3');
    });

    let url;
    if (process.env.ELECTRON_WEBPACK_WDS_HOST) {
        priceViewWindow.webContents.openDevTools();
        url = `http://${process.env.ELECTRON_WEBPACK_WDS_HOST}:${process.env.ELECTRON_WEBPACK_WDS_PORT}#price-view`;
    } else {
        url = `file://${__dirname}/index.html#price-view`;
    }

    priceViewWindow.loadURL(url);
}
