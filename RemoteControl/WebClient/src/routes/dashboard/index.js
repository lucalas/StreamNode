import { Component } from 'preact';

import { Layout, Row, Col, Space, Typography, Divider, Select } from 'antd';
import VolumeBox from '../../components/volumebox';
import SceneBox from '../../components/scenebox';
import WsSocket from '../../services/WSSocketConnector';

const {Title} = Typography;
const { Option } = Select;

const deviceFilterNone = "none";

class Dashboard extends Component {
    sceneList = ['Scena 1','Scena 2','Scena 3'];
    constructor() {
        super();
        this.state = { volumes: [], obsScenes: [], deviceFilter : deviceFilterNone};
    }

    componentDidMount() {
        WsSocket.addConnectHandler(this.onConnect.bind(this));
    }

    onConnect() {
        console.log("connected");
        this.getVolumeTab();
        this.getObsScenesTab();
    }

    getVolumeTab() {
        WsSocket.getVolumes().then(socketVolumes => {
            //console.log(JSON.stringify(socketVolumes.data));
            this.setState({volumes: socketVolumes.data});
        });
    }

    getObsScenesTab() {
        WsSocket.getObsScenes().then(socketObsScenes => {
            this.setState({obsScenes: socketObsScenes.data});
        });
    }

    onVolumeChange(name, deviceName, volume, output, mute) {
        WsSocket.changeVolume(name, deviceName, volume, output, mute);
    }

    onMutePressed(name, deviceName, volume, output, mute) {
        WsSocket.changeVolume(name, deviceName, volume, output, mute);
    }

    onSceneClick(event, name) {
        WsSocket.selectScene(name);
    }

    getGUIVolumes() {
        return this.state.volumes.filter(volume => {
            let valid = true;
            if (this.state.deviceFilter !== deviceFilterNone && volume.output) {
                 valid = volume.device === this.state.deviceFilter;
            }
            return valid;
        }).map(audio => {
            //console.log(JSON.stringify(audio));
            return (<Col span={6} hidden={audio.hidden}>
                        <VolumeBox onVolumeChange={this.onVolumeChange.bind(this)}
                                    onMutePressed={this.onMutePressed.bind(this)}
                                    title={audio.name} volume={audio.volume} deviceName={audio.device} output={audio.output} defaultMute={audio.mute} icon={audio.icon}
                                    onHideEvent={hide => {
                                        audio.hidden = hide; this.setState({volumes: this.state.volumes})
                                        }}/>
                    </Col>)
        });
    }

    getGUIObsScenes() {
        return this.state.obsScenes.map(scene => {
            return (<Col span={6}>
                        <SceneBox onSceneClick={this.onSceneClick.bind(this)} title={scene.name} />
                    </Col>)
        });
    }

    getSourceList() {
        return Array.from(new Set(this.state.volumes.filter(vol => vol.output).map(vol => vol.device))).map(device => <Option key={device} value={device}>{device}</Option>);
    }

    onChangeDevice(device) {
        this.setState({deviceFilter: device});
    }

    render() {
        //console.log(JSON.stringify(this.state.volumes));
        const GUIVolumes = this.getGUIVolumes();
        const GUIObsScenes = this.getGUIObsScenes();
        const optionSourcesList = this.getSourceList();

        return (
            <Layout style={{minHeight:'100vh'}}>

                <Space direction="vertical" size={12}>

                <Divider type="vertical" />

                <Row justify="center">
                    <Title level={2} style={{marginBottom:0}}>MIXER</Title>
                    <Select defaultValue={deviceFilterNone} style={{ width: '100%' }} onChange={this.onChangeDevice.bind(this)}>
                        <Option value={deviceFilterNone}>None</Option>
                        {optionSourcesList}
                    </Select>
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