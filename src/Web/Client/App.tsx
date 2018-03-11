import * as signalR from '@aspnet/signalr';
import * as React from 'react';
import { Fabric } from 'office-ui-fabric-react/lib/Fabric';
import { DefaultButton } from 'office-ui-fabric-react/lib/Button';
import { SidePanel } from './Components/SidePanels/SidePanel';
import { ConnectedCounter } from './Counter';


const logger = new signalR.ConsoleLogger(signalR.LogLevel.Trace);
const transportType = signalR.TransportType.WebSockets;

function connect() {
    let options = { transport: transportType, logging: logger };
    return new signalR.HubConnection('/signalr', options);
}

export class App extends React.Component {
    render() {
        return <Fabric>
            <div className="ms-Grid">
                <div className="ms-Grid-row">
                    <div className="ms-Grid-col ms-sm6 ms-md8 ms-lg10">
                        <ConnectedCounter />
                    </div>
                    <div className="ms-Grid-col ms-sm6 ms-md4 ms-lg2">
                        <SidePanel />
                    </div>
                </div>
            </div>
        </Fabric>;
    }
}

