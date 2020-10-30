import { ipcMain, ipcRenderer } from 'electron';

import { connection } from 'common/SidekickConsole';
import * as ipcEvent from './ipcEvents';

let areMainHooksSetup = false;

export function setupMainHooks() {
    if (areMainHooksSetup) {
        return;
    }

    ipcMain.on(ipcEvent.GET_SETTINGS, async (event) => {
        event.returnValue = await connection?.send(ipcEvent.GET_SETTINGS);
    });

    ipcMain.on(ipcEvent.PARSE_ITEM, async (event, itemText: string) => {
        event.returnValue = await connection?.send(ipcEvent.PARSE_ITEM, itemText);
    });

    areMainHooksSetup = true;
}

export function getSettings() {
    return ipcRenderer.sendSync(ipcEvent.GET_SETTINGS);
}

export function parseItem(itemText: string): any {
    return ipcRenderer.sendSync(ipcEvent.PARSE_ITEM, itemText);
}
