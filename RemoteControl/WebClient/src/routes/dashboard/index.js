import { Component } from 'preact';

import { Layout, Row, Col } from 'antd';

class Dashboard extends Component {

    constructor() {
        super();
    }

    render() {
        return (
        <Layout>
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