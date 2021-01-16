import { Component } from 'preact';

import { Layout, Row, Col } from 'antd';
import VolumeBox from '../../components/volumebox';
import WsSocket from '../../services/WSSocketConnector';


class Dashboard extends Component {
    constructor() {
        super();
        this.state = { volumes: [] };
        this.onConnect = this.onConnect.bind(this);
        WsSocket.addConnectHandler(this.onConnect);
    }

    onConnect() {
        console.log("connected");
        this.getVolumeTab();
    }

    getVolumeTab() {
        WsSocket.getVolumes().then(socketVolumes => {
            console.log(JSON.stringify(socketVolumes.data));
            this.setState({volumes: socketVolumes.data});
        });
    }

    onVolumeChange(title, volume) {
        // TODO call websocket servire to change PC-Server volume
    }

    render() {
        let GUIVolumes;
        console.log(JSON.stringify(this.state.volumes));
        GUIVolumes = this.state.volumes.map(audio => {
            console.log(JSON.stringify(audio));
            return (<Col span={6}><VolumeBox title={audio.name} volume={audio.volume} onVolumeChange={this.onVolumeChange.bind()}/></Col>)
        });

        return (
            <Layout>
                <Row>
                    {GUIVolumes}
                </Row>
                <Row>
                    <Col span={6}>cioa</Col>
                    <Col span={6}>cioa</Col>
                    <Col span={6}>cioa</Col>
                    <Col span={6}>cioa</Col>
                </Row>
                <Row>
                    <Col span={6}>cioa</Col>
                    <Col span={6}>cioa</Col>
                    <Col span={6}>cioa</Col>
                    <Col span={6}>cioa</Col>
                </Row>
            </Layout>
        );
    }
};

export default Dashboard;