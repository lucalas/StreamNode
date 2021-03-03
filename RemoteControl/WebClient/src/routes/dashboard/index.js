import { Component } from 'preact';

import { Layout, Row, Col, Space, Typography, Divider, Select, Spin } from 'antd';
import VolumeBox from '../../components/volumebox';
import SceneBox from '../../components/scenebox';
import WsSocket from '../../services/WSSocketConnector';

const {Title, Text} = Typography;
const { Option } = Select;

const deviceFilterNone = "none";

class Dashboard extends Component {
    sceneList = ['Scena 1','Scena 2','Scena 3'];
    constructor() {
        super();
        this.state = { 
            volumes: [], 
            obsScenes: [], 
            deviceFilter : deviceFilterNone, 
            VolumesLoaded: false, 
            ScenesLoaded: false,
            isMobile: false
        };
    }

    componentDidMount() {
        WsSocket.addConnectHandler(this.onConnect.bind(this));
        if(window.innerWidth <= 480){
            this.setState({isMobile: true});
        }
        window.addEventListener("resize", this._checkWindowSize.bind(this));
    }

    componentWillUnmount(){
        window.removeEventListener("resize", this._checkWindowSize.bind(this));
    }

    _checkWindowSize(){
        if(window.innerWidth <= 480){
            this.setState({isMobile: true});
        }else{
            this.setState({isMobile: false});
        }
    }

    onConnect() {
        console.log("connected");
        this.getVolumeTab()
            .then(()=> this.setState({ VolumesLoaded: true}) )
            .catch(err => console.log(err));
        
        this.getObsScenesTab()
            .then(() => this.setState({ ScenesLoaded: true}) )
            .catch(err => console.log(err));
    }

    async getVolumeTab() {
        await WsSocket.getVolumes().then(socketVolumes => {
            //console.log(JSON.stringify(socketVolumes.data));
            this.setState({volumes: socketVolumes.data});
        });
    }

    async getObsScenesTab() {
        await WsSocket.getObsScenes().then(socketObsScenes => {
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
        if (this.state.volumes.length == 0) {
            return <Row justify="center"> <Text>NO VOLUMES FOUND</Text> </Row>
        }

        return this.state.volumes.filter(volume => {
            let valid = true;
            if (this.state.deviceFilter !== deviceFilterNone && volume.output) {
                 valid = volume.device === this.state.deviceFilter;
            }
            return valid;
        }).map(audio => {
            //console.log(JSON.stringify(audio));
            return (<Col span={this.state.isMobile ? 24 : 6} hidden={audio.hidden}>
                        <VolumeBox onVolumeChange={this.onVolumeChange.bind(this)}
                                    onMutePressed={this.onMutePressed.bind(this)}
                                    title={audio.name} volume={audio.volume} deviceName={audio.device} output={audio.output} defaultMute={audio.mute} icon={audio.icon}
                                    onHideEvent={hide => {audio.hidden = hide}}/>
                    </Col>)
        });
    }

    getGUIObsScenes() {
        if(this.state.obsScenes.length == 0){
            return <Row justify="center"> <Text>NO OBS SCENES FOUND</Text> </Row>
        }

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

                <Row justify="center" id="mixer">
                    <Title level={2} style={{marginBottom:0}}>MIXER</Title>
                </Row>

                <Row justify="center" align="middle">
                    <Col span={this.state.isMobile ? 3 : 2} >
                        <Row justify="center">
                        <Text strong >Filtro: </Text>
                        </Row>
                    </Col>
                    <Col span={this.state.isMobile ? 18 : 20}>
                        
                        <Select defaultValue={deviceFilterNone} onChange={this.onChangeDevice.bind(this)} style={{width: "100%"}}>
                            <Option value={deviceFilterNone}>None</Option>
                            {optionSourcesList}
                        </Select>
                    </Col>
                </Row>

                {   //VOLUMES DECK
                    this.state.VolumesLoaded 
                    
                    ?

                    <Row justify={"start"}>
                        {GUIVolumes}
                    </Row>

                    :

                    <Row justify="center">
                        <Col><Spin/></Col>
                    </Row>
                }

                <Divider type="vertical" />
                
                <Row justify="center" id="obs-scenes">
                    <Title level={2} style={{marginBottom:0}}>SCENE</Title>  
                </Row>

                {   //OBS SCENES
                    this.state.ScenesLoaded 

                    ?

                    <Row justify={this.state.obsScenes.length == 0 ? "center" : "start"}>
                        {GUIObsScenes}
                    </Row>

                    :

                    <Row justify="center">
                        <Col><Spin/></Col>
                    </Row>
                }

                <Divider type="vertical" />

                </Space>
            </Layout>
        );
    }
};

export default Dashboard;