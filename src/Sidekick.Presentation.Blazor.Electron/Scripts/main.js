const { BrowserWindow } = require('electron');

const win = new BrowserWindow();
win.webContents.openDevTools();
