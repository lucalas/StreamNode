import { Component, createRef } from 'preact';

import { Card, Slider, Button, Row, Col, Image, Avatar, Menu, Dropdown, Typography } from 'antd';
import { SoundOutlined, LockOutlined, UnlockOutlined, AudioOutlined, AudioMutedOutlined, EllipsisOutlined } from '@ant-design/icons';

const { Text } = Typography;


class VolumeBox extends Component {
    _slider = createRef();


    constructor(props) {
        super(props);
        this.state = {
            mute: this.props.defaultMute,
            audioLocked: false,
            micLocked: false,
            hide: false
        }
    }

    _muteSound(){
        let newMuteValue = !this.state.mute;
        this.setState({
            mute: newMuteValue
        });
        this.props.onMutePressed(this.props.title, this.props.deviceName, this._slider.current.state.value, this.props.output, newMuteValue);
    }

    _lockSound(){
        this.setState({
            audioLocked: !this.state.audioLocked
        });
    }

    getVolumeAudioIcon() {
        if (this.props.output) {
            return <SoundOutlined />;
        } 
        return !this.state.mute ? <AudioOutlined /> : <AudioMutedOutlined />;
    }

    getAudioIcon() {
        let Icon = null;
        if (this.props.output) {
            if (this.props.icon) {
                Icon = <Image src={this.props.icon} width={this.props.isVertical ? 32 : 64} preview={false} />;
            } else {
                Icon = <Avatar size={this.props.isVertical ? 32 : 64} icon={<SoundOutlined/>} />;
            }
        } else {
            if (this.state.mute) {
                Icon = <Avatar size={this.props.isVertical ? 32 : 64} icon={<AudioMutedOutlined height="auto" />} />;
            } else {
                Icon = <Avatar size={this.props.isVertical ? 32 : 64} icon={<AudioOutlined />} />;
            }
        }
        return Icon;
    }

    setHideValue(value) {
        this.setState({hide: value});
        this.props.onHideEvent(value);
    }

    getMenu() {
        return (<Menu>
                    <Menu.Item>
                        <Text onClick={event => this.setHideValue(!this.state.hide)}>Hide me</Text>
                    </Menu.Item>
                </Menu>)
    }

    render() {
        return (
            !this.props.isVertical
            ?

            // HORIZONTAL
            <Card 
                title={this.props.title + " " + this.props.deviceName} 
                headStyle={{textAlign: 'center', backgroundColor:'rgba(24,144,255,0.8)', color:'white'}}
                style={{borderRight: '1px solid #f2f2f2'}}
                bordered={false}
                extra={
                    <Dropdown overlay={this.getMenu()}>
                        <EllipsisOutlined style={"transform: rotate(90deg);"} />
                    </Dropdown>
                }
            >
                <Row justify="center">
                    {this.getAudioIcon()}
                </Row>

                <Col>
                    <Row justify="" hidden={this.props.volumeHide}>
                        <Button 
                            type={!this.state.mute ? "primary" : "default"}
                            shape="circle" 
                            icon={this.getVolumeAudioIcon()} 
                            onClick={this._muteSound.bind(this)}
                        />
                        
                        <Slider
                            className="slider-test-css-t"
                            ref={this._slider}
                            min={0} 
                            max={100}
                            onChange={this.onVolumeChange?.bind(this)}
                            defaultValue={this.props.volume} 
                            disabled={this.state.audioLocked}
                            style={{
                                width: "75%"
                            }}
                        />
                    </Row>
                </Col>
            </Card>
            :

            //VERTICAL
           <Card 
            title={
                <Row justify="center">
                    {this.getAudioIcon()}
                </Row>
            } 
            headStyle={{textAlign: 'center', backgroundColor:'rgba(24,144,255,0.8)', color:'white'}}
            style={{borderRight: '1px solid #f2f2f2'}}
            bodyStyle={{height: "auto", display: "flex", justifyContent:"center"}}
            bordered={false}
           >
            <Col>
                <Row style={{margin: "10px 0"}}>
                <Slider vertical
                        ref={this._slider} 
                        min={0} 
                        max={100} 
                        onChange={this.onVolumeChange.bind(this)}
                        defaultValue={this.props.volume} 
                        disabled={this.state.audioLocked} 
                        style={{ height:"200px"}}
                    />
                </Row>
                <Row style={{margin: "10px 0"}}>
                    <Button 
                        type={!this.state.mute ? "primary" : "default"}
                        shape="circle" 
                        icon={this.getVolumeAudioIcon()} 
                        onClick={this._muteSound.bind(this)}
                    />
                </Row>
            </Col>
           </Card>
        );
    }


    timeoutChangeVolume = null;
    onVolumeChange(value) {
        // If timeout isn't passed we cancel the request and create new one to avoid flodding of requests
        if (this.timeoutChangeVolume !== null) clearTimeout(this.timeoutChangeVolume);
        this.timeoutChangeVolume = setTimeout(() => {
            if (this.props.onVolumeChange !== undefined) {
                this.props.onVolumeChange(this.props.title, this.props.deviceName, value, this.props.output, this.state.mute);
            }
        // We found that a good value of delay to have a gradual change of the volume is 150
        },150);
    }
}

export default VolumeBox;