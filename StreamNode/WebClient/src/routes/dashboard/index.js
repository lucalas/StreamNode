import { Component } from 'preact';

import { Layout, Row, Col, Space, Typography, Divider, Select, Spin, Switch, Button } from 'antd';
import { EditOutlined, UndoOutlined, CheckOutlined } from '@ant-design/icons'
import VolumeBox from '../../components/volumebox';
import SceneBox from '../../components/scenebox';
import WsSocket from '../../services/WSSocketConnector';

//REACT DND
import { DndProvider, useDrop } from 'react-dnd'
import { HTML5Backend } from 'react-dnd-html5-backend'


const { Title, Text } = Typography;
const { Option } = Select;

const deviceFilterNone = "none";

class Dashboard extends Component {

    backupDeck = [];
    constructor() {
        super();
        this.state = {
            volumes: [],
            obsScenes: [],
            deviceFilter: deviceFilterNone,
            VolumesLoaded: false,
            ScenesLoaded: false,
            isMobile: false,
            //STATE FOR TOOGLE BUTTON: VERTICAL OR HORIZONTAL CARD
            isVertical: true,
            //STATE TO CONTROL EDITABLE MODE
            isEditable: false
        };
    }

    componentDidMount() {
        WsSocket.addVolumeUpdateHandler(this.getVolumeUpdate.bind(this));

        WsSocket.addConnectHandler(this.onConnect.bind(this));
        if (window.innerWidth <= 480) {
            this.setState({ isMobile: true });
        }

        window.addEventListener("resize", this._checkWindowSize.bind(this));
    }

    componentWillUnmount() {
        window.removeEventListener("resize", this._checkWindowSize.bind(this));
    }

    _checkWindowSize() {
        if (window.innerWidth <= 480) {
            this.setState({ isMobile: true });
        } else {
            this.setState({ isMobile: false });
        }
    }

    onConnect() {
        console.log("connected");
        this.getVolumeTab()
            .then(() => this.setState({ VolumesLoaded: true }))
            .catch(err => console.log(err));

        this.getObsScenesTab()
            .then(() => this.setState({ ScenesLoaded: true }))
            .catch(err => console.log(err));
    }

    getVolumeUpdate(socketVolumes) {
        this.setState({ volumes: socketVolumes.data });
    }

    async getVolumeTab() {
        await WsSocket.getVolumes().then(socketVolumes => {
            this.setState({ volumes: socketVolumes.data });
        });
    }

    async getObsScenesTab() {
        await WsSocket.getObsScenes().then(socketObsScenes => {
            this.setState({ obsScenes: socketObsScenes.data });
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

        return this.state.volumes
        .sort((a, b) => a.order - b.order)
        .filter(volume => {
            let valid = true;
            if (this.state.deviceFilter !== deviceFilterNone && volume.output) {
                valid = volume.device === this.state.deviceFilter;
            }
            return valid;
        }).map((audio, idx) => {
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
                    id="volumeBox"
                >
                    <VolumeBox onVolumeChange={this.onVolumeChange.bind(this)}
                        onMutePressed={this.onMutePressed.bind(this)}
                        title={audio.name} volume={audio.volume} deviceName={audio.device} output={audio.output} defaultMute={audio.mute} icon={audio.icon}
                        onHideEvent={hide => { audio.hidden = hide }}
                        isVertical={this.state.isVertical}
                        isEditable={this.state.isEditable}
                        dropEvent={this.onDroppedEvent.bind(this)}
                        index={idx}
                    />
                </Col>
            )
        });
    }

    getGUIObsScenes() {
        if (this.state.obsScenes.length == 0) {
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
        this.setState({ deviceFilter: device });
    }

    onDroppedEvent(fromIndex, toIndex) {
        if (fromIndex === toIndex) return;
        const data = [...this.state.volumes];
        const item = data.splice(fromIndex, 1)[0];
        //console.log(fromIndex, toIndex);
        data.splice(toIndex, 0, item);
        data.forEach((vl, index) => vl.order = index);
        this.setState({ volumes: data });
    }

    makeEditable() {
        this.backupDeck = this.state.volumes.slice();
        this.setState({ isEditable: true });
    }

    saveDeckState() {
        WsSocket.storeDeck(this.state.volumes
            .map((vol, index) => {return{id: vol.device + vol.name, order: vol.order != -1 ? vol.order : index}}));
        this.setState({ isEditable: false });
    }

    undoDeckState() {
        this.setState({ isEditable: false, volumes: this.backupDeck });
        this.backupDeck = [];
    }

    render() {
        //console.log(JSON.stringify(this.state.volumes));
        const GUIVolumes = this.getGUIVolumes();
        const GUIObsScenes = this.getGUIObsScenes();
        const optionSourcesList = this.getSourceList();

        return (
            <Layout style={{ minHeight: '100vh' }}>
                <Space direction="vertical" size={12}>

                    <Divider type="vertical" />

                    <Row justify="center" id="mixer">
                        <Col>
                            <Row justify="center">
                                <Title level={2} style={{ marginBottom: 0 }}>MIXER</Title>
                            </Row>

                            {this.state.isEditable &&
                                <Row justify="center">
                                    <Title level={4} style={{ marginBottom: 0 }}>(Edit Mode)</Title>
                                </Row>
                            }
                        </Col>
                    </Row>

                    {
                        <Row justify={this.state.isMobile ? "start" : "center"} align="middle">
                            {
                                !this.state.isMobile &&
                                <Col xs={4} sm={3} md={3} lg={2} xl={1}>
                                    <Row justify="center">
                                        <Text strong >Filtro: </Text>
                                    </Row>
                                </Col>
                            }
                            <Col offset={this.state.isMobile && 1} xs={15} sm={16} md={17} lg={18} xl={18}>
                                <Select defaultValue={deviceFilterNone} onChange={this.onChangeDevice.bind(this)} style={{ width: "100%" }}>
                                    <Option value={deviceFilterNone}>All</Option>
                                    {optionSourcesList}
                                </Select>
                            </Col>
                            <Col offset={this.state.isMobile ? 2 : 1} span={1}>
                                <Switch onChange={() => this.setState({ isVertical: !this.state.isVertical })}
                                    checkedChildren="V"
                                    unCheckedChildren="H"
                                    defaultChecked={this.state.isVertical}
                                />
                            </Col>
                            {!this.state.isMobile &&
                                <Col >
                                {
                                    !this.state.isEditable 
                                    ?
                                    <Row align="middle">
                                        <Button
                                            icon={<EditOutlined />}
                                            type={this.state.isEditable ? "primary" : "default"}
                                            size={32}
                                            shape="circle"
                                            onClick={this.makeEditable.bind(this)}
                                        />
                                        <p style={{ margin: "0 0 0 5px" }}>Edit Mode</p>
                                    </Row>
                                    :
                                    <Row align="middle">
                                        <Button icon={<CheckOutlined />} size={32} shape="circle" type="primary" onClick={this.saveDeckState.bind(this)}/>
                                        <Button icon={<UndoOutlined/>} size={32} shape="circle" type="primary" danger  onClick={this.undoDeckState.bind(this)}/>
                                        <p style={{ margin: "0 0 0 5px" }}>Edit Mode</p>
                                    </Row>
                                }
                                </Col>
                            }
                        </Row>
                    }

                    {
                        this.state.VolumesLoaded

                            ?

                            <DndProvider backend={HTML5Backend}>
                                <Row
                                    justify={"start"}
                                    style={{ margin: this.state.isVertical ? (!this.state.isMobile ? "0 5%" : "0 auto") : "0 auto" }}
                                >
                                    {GUIVolumes}
                                </Row>
                            </DndProvider>

                            :

                            <Row justify="center">
                                <Col><Spin /></Col>
                            </Row>

                    }

                    <Divider type="vertical" />

                    <Row justify="center" id="obs-scenes">
                        <Title level={2} style={{ marginBottom: 0 }}>SCENE</Title>
                    </Row>

                    {   //OBS SCENES
                        this.state.ScenesLoaded

                            ?

                            <Row justify={this.state.obsScenes.length == 0 ? "center" : "start"}>
                                {GUIObsScenes}
                            </Row>

                            :

                            <Row justify="center">
                                <Col><Spin /></Col>
                            </Row>
                    }

                    <Divider type="vertical" />

                </Space>
            </Layout>
        );
    }
};

export default Dashboard;