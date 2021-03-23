
const dataType = {
    volumes: "volumes",
    obs: "obs",
    changeVolume: "change-volume",
    changeObs: "change-obs",
    volumeUpdate: "volume-update",
    storeDeck: "store-deck"
}

class WSSocketConnector {
    connection = null;
    onopen = null;
    onVolumeGetCallbacks = [];
    onObsScenesCallbacks = [];
    onVolumeUpdate = [];
    onConnectHandler = [];
    reconnectionInterval = 2000;
    timeoutInterval = null;
    host = location.hostname;
    port = 8189;

    constructor() {
        this.connect();
    }

    
    connect() {
        return new Promise((resolve, reject) => {
            this.connection = new WebSocket('ws://' + this.host + ':' + this.port);
            this.connection.onopen = () => {
                this.onConnectHandler.forEach(method => method());
                if (this.exists(this.onopen)) this.onopen();
                resolve();
            };

            this.connection.onmessage = data => {
                if (this.exists(this.onmessage)) this.onmessage(data);
            }

            this.connection.onclose = event => {
                this.timeoutInterval = setTimeout(() => {
                    console.log("trying to reconnect");
                    this.connect(this.host, this.port);
                  }, this.reconnectionInterval);
            }
        });
    }

    addVolumeUpdateHandler(method) {
        this.onVolumeUpdate.push(method);
    }

    addConnectHandler(method) {
        this.onConnectHandler.push(method);
    }

    getConnectionStatus() {
        return this.connection.readyState;
    }

    onmessage(message) {
        let data = JSON.parse(message.data);

        if (data.type === dataType.volumes) {
            this.onVolumeGetCallbacks.forEach(ele => { ele.callback(data)});
        } else if (data.type === dataType.obs) {
            this.onObsScenesCallbacks.forEach(ele => { ele.callback(data)});
        } else if (data.type === dataType.volumeUpdate) {
            this.onVolumeUpdate.forEach(ele => { ele(data)});
        }
    }
    
    sendData(message) {
        this.connection.send(JSON.stringify(message));
    }

    storeDeck(data2Store) {
        this._getMessageAckData(dataType.storeDeck, data2Store, []);
    }

    changeVolume(name, deviceName, volume, output, mute) {
        let req = this.createRequest(dataType.changeVolume);
        req.data = { name: name, volume: volume, device: deviceName, output, mute: mute }
        this.sendData(req);
    }

    selectScene(name) {
        let req = this.createRequest(dataType.changeObs);
        req.data = {name: name};
        this.sendData(req);
    }

    getVolumes() {
        return this._getData(dataType.volumes, this.onVolumeGetCallbacks);
    }

    getObsScenes() {
        return this._getData(dataType.obs, this.onObsScenesCallbacks);
    }

    _getData(type, callbacks) {
        return this._getMessageAckData(type, null, callbacks);
    }

    _getMessageAckData(type, data, callbacks) {
        return new Promise((resolve, reject) => {
            let id = type + new Date().getTime();

            // Handler to get volumes data from a websocket response that is asynchronous
            callbacks.push({id: id, callback: data => {
                callbacks = callbacks.filter(ele => { ele.id !== id});
                resolve(data);
            }});
            let req = this.createRequest(type);
            req.data = data;
            this.sendData(req);
        });
    }

    /**
     * Utils method.
     */
    createRequest(type) {
        return {"type": type};
    }

    exists(obj) {
        return (obj != undefined && obj != null)
    }
};

const WsSocket = new WSSocketConnector();

export default WsSocket;