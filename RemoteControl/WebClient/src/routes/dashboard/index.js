import { Component } from 'preact';

import { Layout, Row, Col, Space, Typography, Divider } from 'antd';
import VolumeBox from '../../components/volumebox';
import SceneBox from '../../components/scenebox';
import WsSocket from '../../services/WSSocketConnector';

const {Title} = Typography;

class Dashboard extends Component {
    sceneList = ['Scena 1','Scena 2','Scena 3'];
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

    onVolumeChange(name, deviceName, volume) {
        // TODO call websocket servire to change PC-Server volume
        WsSocket.changeVolume(name, deviceName, volume);
    }

    render() {
        let GUIVolumes;
        console.log(JSON.stringify(this.state.volumes));
        GUIVolumes = this.state.volumes.map(audio => {
            console.log(JSON.stringify(audio));
            return (<Col span={6}>
                        <VolumeBox title={audio.name} volume={audio.volume} onVolumeChange={this.onVolumeChange.bind()} deviceName={audio.device}/>
                    </Col>)
        });

        return (
            <Layout style={{minHeight:'100vh'}}>

                <Space direction="vertical" size={12}>

                <Divider type="vertical" />

                <Row justify="center">
                    <Title level={2} style={{marginBottom:0}}>MIXER</Title>  
                </Row>

                <Row>
                    {GUIVolumes}
                </Row>

                <Divider type="vertical" />
                
                <Row justify="center">
                    <Title level={2} style={{marginBottom:0}}>SCENE</Title>  
                </Row>

                <Row>
                    {
                        this.sceneList.map( elem => 
                            <Col span={6}>
                                <SceneBox title={elem}/>
                            </Col>
                        )
                    }
                </Row>
                </Space>
            </Layout>
        );
    }
};

export default Dashboard;