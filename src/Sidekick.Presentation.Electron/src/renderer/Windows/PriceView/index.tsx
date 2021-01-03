import React, { useEffect, useState } from 'react';
import { ipcRenderer as ipc, remote } from 'electron';
import classNames from 'classnames';
import * as B from '@blueprintjs/core';

import { getStatic } from 'common/Utils';

import '@blueprintjs/core/lib/css/blueprint.css';
import './style.scss';

const icon = getStatic('icon.png');

function PriceView() {
    const [item, setItem] = useState<any>(null);
    const [isLoading, setIsLoading] = useState(false);
    const [isError, setIsError] = useState(false);

    useEffect(function onInitialize() {
        ipc.on('show', showWindow);
        ipc.on('close', closeWindow);
        ipc.on('start-loading', () => {
            setIsError(false);
            setIsLoading(true);
        });
    });

    useEffect(
        function receiveItem() {
            ipc.on('item-parsed', (e, response) => {
                setIsLoading(false);
                setItem(response);
                setIsError(response === null);
            });
        },
        [setItem]
    );

    function showWindow() {
        remote.getCurrentWindow().showInactive();
    }

    function closeWindow() {
        remote.getCurrentWindow().hide();
        setIsLoading(false);
        setItem(null);
    }

    return (
        <div className="blueprint-view bp3-dark">
            <B.Navbar style={{ backgroundColor: B.Colors.BLACK }} className="window-tab">
                <B.Navbar.Group className="window-tab__handle" align={B.Alignment.LEFT}>
                    <img className="window-logo" src={icon} />
                    <B.Navbar.Heading className="window-title">Sidekick</B.Navbar.Heading>
                </B.Navbar.Group>
                <B.Navbar.Group align={B.Alignment.RIGHT}>
                    <B.Button tabIndex={-1} minimal icon="cross" onClick={closeWindow} />
                </B.Navbar.Group>
            </B.Navbar>

            <div className="content">
                <div className={classNames('item', { 'item--has-item': item !== null })}>
                    {item && (
                        <>
                            <p>{item.name || item.nameLine}</p>
                            <p>{item.type}</p>
                        </>
                    )}
                </div>

                {isError && (
                    <B.Callout icon="error" intent={B.Intent.DANGER}>
                        Could not parse item.
                    </B.Callout>
                )}

                {isLoading && <B.Spinner className="item-loader" size={B.Spinner.SIZE_SMALL} />}
            </div>
        </div>
    );
}

export default PriceView;
