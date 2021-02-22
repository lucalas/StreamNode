import React, { Component } from 'react'
import { Card, Col, Row, Typography, Image } from 'antd';
import ObsSceneIcon from '../../assets/icons/OBS.svg';

const {Title} = Typography;

class SceneBox extends Component {
    constructor(props){
        super(props);
        console.log(ObsSceneIcon);
    }

    render() {
        return (
            <Card onClick={this.onClick.bind(this)}>
                <Col span={24}>
                    <Row justify="center">
                        <Title level={5}>{this.props.title}</Title>
                    </Row>
                    <Row justify="center">
                        <Image src={ObsSceneIcon} width={100}/>
                    </Row>
                </Col>
            </Card>
        )
    }

    onClick(event) {
        if (this.props.onSceneClick) {
            this.props.onSceneClick(event, this.props.title);
        }
    }
}

export default SceneBox;