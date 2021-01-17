import React, { Component } from 'react'
import { Card, Avatar, Col, Row, Typography } from 'antd';

const {Title} = Typography;

class SceneBox extends Component {
    constructor(props){
        super(props);
    }

    render() {
        return (
            <Card>
                <Col span={24}>  
                    <Row justify="center">
                        <Title level={5}>{this.props.title}</Title>
                    </Row>
                    <Row justify="center">
                        <Avatar shape="square" size={128}/>
                    </Row>
                    
                </Col>
            </Card>
        )
    }
}

export default SceneBox;