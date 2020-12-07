const WSSocketConnector = {
    connection : null,
    onopen : null,
    onmessage : null,

    connect : (ip, port) => {
        this.connection = new WebSocket('ws://' + ip + ':' + port);
        this.connection.onopen = () => {
            if (this.exists(this.onopen)) this.onopen();
        };

        this.connection.onmessage = data => {
            if (this.exists(this.onmessage)) this.onmessage(data);
        }
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