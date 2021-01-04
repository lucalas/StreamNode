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

    connect(ip, port) {
        return new Promise((resolve, reject) => {
            this.connection = new WebSocket('ws://' + ip + ':' + port);
            this.connection.onopen = () => {
                if (this.exists(this.onopen)) this.onopen();
                resolve();
            };

            this.connection.onmessage = data => {
                if (this.exists(this.onmessage)) this.onmessage(data);
            }
        });
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

    getVolumes() {
        return new Promise((resolve, reject) => {
            let id = new Date().getTime();
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

export default WSSocketConnector;