import { ConnectionBuilder, Connection } from 'electron-cgi';

import { isDevelopment } from './Utils';

export let connection: Connection | undefined;

/**
 * Launch and connect to the `Sidekick.Console` application.
 */
export function connect() {
    if (connection !== undefined) {
        return;
    }

    connection = isDevelopment
        ? new ConnectionBuilder().connectTo('dotnet', 'run', '--project', '../Sidekick.Console').build()
        : new ConnectionBuilder().connectTo('./Sidekick.Console/Sidekick.Console.exe').build();

    connection.onDisconnect = () => {
        console.log('TODO');
    };
}
