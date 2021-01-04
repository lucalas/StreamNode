import { Component } from 'preact';

import { Layout, Row, Col } from 'antd';
import VolumeBox from '../../components/volumebox';
import WSSocketConnector from '../../services/WSSocketConnector';


class Dashboard extends Component {

    wssocket = new WSSocketConnector();

    constructor() {
        super();
        this.connectWS();
    }

    connectWS() {
        console.log("connect test");
        this.wssocket.connect("127.0.0.1", "8181")
        .then(() => {
            this.wssocket.getVolumes();
        });
    }

    render() {
        return (
            <Layout>
                <Row>
                    <Col span={6}><VolumeBox/></Col>
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