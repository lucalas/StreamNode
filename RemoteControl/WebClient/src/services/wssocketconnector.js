const WSSocketConnector = {
    connection : null,
    onopen : null,
    onvolumechange : null,

    connect : (ip, port) => {
        this.connection = new WebSocket('ws://' + ip + ':' + port);
        this.connection.onopen = () => {
            if (this.exists(this.onopen)) this.onopen();
        };

        this.connection.onmessage = data => {
            if (this.exists(this.onmessage)) this.onmessage(data);
        }
    },

    onmessage : (message) => {
        let data = JSON.parse(message);
    },

    sendData : (message) => {
        this.connection.send(message);
    },

    /**
     * Utils method.
     */
    exists : (obj) => {
        return (obj != undefined && obj != null)
    }
};

export default WSSocketConnector;