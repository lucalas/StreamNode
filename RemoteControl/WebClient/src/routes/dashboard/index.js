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
        this.state = { volumes: [], obsScenes: []};
        this.onConnect = this.onConnect.bind(this);
        WsSocket.addConnectHandler(this.onConnect);
    }

    onConnect() {
        console.log("connected");
        this.getVolumeTab();
        this.getObsScenesTab();
    }

    getVolumeTab() {
        WsSocket.getVolumes().then(socketVolumes => {
            console.log(JSON.stringify(socketVolumes.data));
            this.setState({volumes: socketVolumes.data});
        });
    }

    getObsScenesTab() {
        WsSocket.getObsScenes().then(socketObsScenes => {
            console.log(JSON.stringify(socketObsScenes.data));
            this.setState({obsScenes: socketObsScenes.data});
        });
    }

    onVolumeChange(name, deviceName, volume, output, mute) {
        WsSocket.changeVolume(name, deviceName, volume, output, mute);
    }

    onMutePressed(name, deviceName, volume, output, mute) {
        WsSocket.changeVolume(name, deviceName, volume, output, mute);
    }

    onSceneClick(name) {
        // TODO
    }

    getGUIVolumes() {
        return this.state.volumes.map(audio => {
            console.log(JSON.stringify(audio));
            return (<Col span={6}>
                        <VolumeBox onVolumeChange={this.onVolumeChange.bind(this)}
                                    onMutePressed={this.onMutePressed.bind(this)}
                                    title={audio.name} volume={audio.volume} deviceName={audio.device} output={audio.output} defaultMute={audio.mute} icon={audio.icon}/>
                    </Col>)
        });
    }

    getGUIObsScenes() {
        return this.state.obsScenes.map(scene => {
            console.log(JSON.stringify(scene));
            return (<Col span={6}>
                        <SceneBox onSceneClick={this.onSceneClick.bind(this)} title={scene.name} />
                    </Col>)
        });
    }

    render() {
        console.log(JSON.stringify(this.state.volumes));
        const GUIVolumes = this.getGUIVolumes();
        const GUIObsScenes = this.getGUIObsScenes();

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
                    {GUIObsScenes}
                </Row>
                </Space>
            </Layout>
        );
    }
};

export default Dashboard;