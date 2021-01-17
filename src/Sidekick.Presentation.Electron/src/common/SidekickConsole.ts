import { ConnectionBuilder, Connection } from 'electron-cgi';

import { isDevelopment } from './Utils';

export let connection: Connection | undefined;

/**
 * Launch and connect to the `Sidekick.Presentation.ElectronProgram` application.
 */
export function connect() {
    if (connection !== undefined) {
        return;
    }

    connection = isDevelopment
        ? new ConnectionBuilder().connectTo('dotnet', 'run', '--project', '../Sidekick.Presentation.ElectronProgram').build()
        : new ConnectionBuilder().connectTo('./Sidekick.Presentation.ElectronProgram/Sidekick.Presentation.ElectronProgram.exe').build();

    connection.onDisconnect = () => {
        console.log('TODO');
    };
}
