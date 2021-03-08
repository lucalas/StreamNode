import { Component } from 'preact';

import { Layout, Row, Col, Space, Typography, Divider, Select, Spin, Switch } from 'antd';
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
            isMobile: false,
            //STATE FOR TOOGLE BUTTON: VERTICAL OR HORIZONTAL CARD
            isVertical: true
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
            return (
                <Col 
                    xs={this.state.isVertical ? 8 : 24} 
                    sm={this.state.isVertical ? 8 : 12} 
                    md={this.state.isVertical ? 4 : 12}
                    lg={this.state.isVertical ? 4 : 8}
                    xl={this.state.isVertical ? 2 : 6}
                    xxl={this.state.isVertical ? 2 : 4}
                    hidden={audio.hidden}
                >
                    <VolumeBox onVolumeChange={this.onVolumeChange.bind(this)}
                                onMutePressed={this.onMutePressed.bind(this)}
                                title={audio.name} volume={audio.volume} deviceName={audio.device} output={audio.output} defaultMute={audio.mute} icon={audio.icon}
                                onHideEvent={hide => {audio.hidden = hide}}
                                isVertical={this.state.isVertical}
                    />
                </Col>
            )
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

                { //FILTRI - TODO Quando è mobile togliere la parola filtro oppure trovare un componente switch verticale
                <Row justify= {this.state.isMobile ? "start" : "center"} align="middle">
                    {
                        !this.state.isMobile &&
                        <Col xs={4} sm={3} md={3} lg={2} xl={1}>
                            <Row justify="center">
                                <Text strong >Filtro: </Text>
                            </Row>
                        </Col>    
                    }
                    <Col offset={this.state.isMobile && 1 } xs={15} sm={16} md={17} lg={18} xl={18}>
                        <Select defaultValue={deviceFilterNone} onChange={this.onChangeDevice.bind(this)} style={{width: "100%"}}>
                            <Option value={deviceFilterNone}>All</Option>
                            {optionSourcesList}
                        </Select>
                    </Col>
                    <Col offset={this.state.isMobile ? 2: 1} span={2}>
                        <Switch onChange={ () => this.setState({ isVertical: !this.state.isVertical }) } 
                            checkedChildren="V"
                            unCheckedChildren="H"
                            defaultChecked={this.state.isVertical}
                        />
                    </Col>
                </Row>
                }

                {   //VOLUMES DECK TODO SETTARE IL MARGINE QUANDO E' SU MOBILE - RIMETTERE A POSTO IL MARGINE E IN CASO ANCHE LE COL SE SI METTONO I MARGINI
                    this.state.VolumesLoaded 
                    
                    ?

                    <Row justify={"start"} style={{ margin: this.state.isVertical ? (!this.state.isMobile ? "0 5%" : "0 auto") : "0 auto"}}>
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