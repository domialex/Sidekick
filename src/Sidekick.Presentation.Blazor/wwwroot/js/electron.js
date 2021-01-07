const { BrowserWindow } = require('electron');
alert('test');

const win = new BrowserWindow();
alert('test2');

win.webContents.openDevTools();
alert('test3');
