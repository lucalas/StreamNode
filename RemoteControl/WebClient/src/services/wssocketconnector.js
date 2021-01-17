const dataType = {
    volumes: "volumes",
    obs: "obs",
    changeVolume: "change-volume",
    changeObs: "change-obs"
}
class WSSocketConnector {
    connection = null;
    onopen = null;
    onvolumechange = null;
    onvolumegetCallbacks = [];
    onconnectHandler = [];

    constructor() {
        this.connect("127.0.0.1", "8189");
    }

    
    connect(ip, port) {
        return new Promise((resolve, reject) => {
            this.connection = new WebSocket('ws://' + ip + ':' + port);
            this.connection.onopen = () => {
                this.onconnectHandler.forEach(method => method());
                if (this.exists(this.onopen)) this.onopen();
                resolve();
            };

            this.connection.onmessage = data => {
                if (this.exists(this.onmessage)) this.onmessage(data);
            }
        });
    }

    addConnectHandler(method) {
        this.onconnectHandler.push(method);
    }

    getConnectionStatus() {
        return this.connection.readyState;
    }

    onmessage(message) {
        let data = JSON.parse(message.data);

        if (data.type === dataType.volumes) {
            this.onvolumegetCallbacks.forEach(ele => { ele.callback(data)});
        }
    }

    sendData(message) {
        this.connection.send(JSON.stringify(message));
    }

    changeVolume(name, deviceName, volume) {
        let req = this.createRequest(dataType.changeVolume);
        req.data = { name: name, volume: volume, device: deviceName }
        this.sendData(req);
    }

    getVolumes() {
        return new Promise((resolve, reject) => {
            let id = new Date().getTime();

            // Handler to get volumes data from a websocket response that is asynchronous
            this.onvolumegetCallbacks.push({id: id, callback: data => {
                this.onvolumegetCallbacks = this.onvolumegetCallbacks.filter(ele => { ele.id !== id});
                resolve(data);
            }});
            this.sendData(this.createRequest(dataType.volumes));
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