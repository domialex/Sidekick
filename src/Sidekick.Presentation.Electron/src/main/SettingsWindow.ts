import { BrowserWindow } from 'electron';

export let settingsWindow: BrowserWindow | undefined;

export function createWindow() {
    if (settingsWindow) {
        try {
            settingsWindow.focus();
        } catch {
            settingsWindow = undefined;
            createWindow();
        }
        return;
    }

    settingsWindow = new BrowserWindow({
        transparent: true,
        focusable: true,
        webPreferences: {
            nodeIntegration: true,
            enableRemoteModule: true,
            webSecurity: false,
        },
        frame: false,
    });

    let url: string;
    if (process.env.ELECTRON_WEBPACK_WDS_HOST) {
        settingsWindow.webContents.openDevTools();
        url = `http://${process.env.ELECTRON_WEBPACK_WDS_HOST}:${process.env.ELECTRON_WEBPACK_WDS_PORT}#settings`;
    } else {
        url = `file://${__dirname}/index.html#settings`;
    }

    settingsWindow.loadURL(url);
}
