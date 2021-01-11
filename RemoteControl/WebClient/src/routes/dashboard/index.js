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

    render() {
        let GUIVolumes;
        console.log(JSON.stringify(this.state.volumes));
        for (let volume in this.state.volumes) {
            console.log(JSON.stringify(volume));
            GUIVolumes += <Col span={6}><VolumeBox title={volume.name}/></Col>
        }

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