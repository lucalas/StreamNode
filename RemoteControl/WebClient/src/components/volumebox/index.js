import { Component, createRef } from 'preact';

import { Card, Slider, Button, Row, Avatar, Col } from 'antd';
import { SoundOutlined, LockOutlined, UnlockOutlined, AudioOutlined, AudioMutedOutlined } from '@ant-design/icons';


class VolumeBox extends Component {
    _slider = createRef();

    constructor(props) {
        super(props);
        this.state = {
            audio: false,
            audioLocked: false,
            micLocked: false
        }
    }

    _muteSound(){
        this.setState({
            audio: !this.state.audio
        });
        this.props.onMutePressed(this.props.title, this.props.deviceName, this._slider.current.state.value, this.props.output, this.state.audio);
    }

    _lockSound(){
        this.setState({
            audioLocked: !this.state.audioLocked
        });
    }

    getVolumeAudioIcon() {
        if (this.props.output) {
            return this.state.audio ? <SoundOutlined/> : <SoundOutlined/>
        } else {
            return this.state.audio ? <AudioOutlined/> : <AudioMutedOutlined/>
        }
    }

    render() {
        return (
            <Card 
                title={this.props.title} 
                headStyle={{textAlign: 'center', backgroundColor:'rgba(24,144,255,0.8)', color:'white'}}
                style={{borderRight: '1px solid #f2f2f2'}}
                bordered={false}
            >
                <Row justify="center">
                    <Avatar size={64}/>
                </Row>
                
                <Col>
                <Row justify="space-between" style={{marginBottom: 5}} hidden={this.props.volumeHide}>
                    <Button 
                        type={this.state.audio ? "default" : "primary"}
                        shape="circle" 
                        icon={this.getVolumeAudioIcon()} 
                        onClick={this._muteSound.bind(this)}
                    />
                    <Button 
                        type={this.state.audioLocked ? "primary" : "default"}
                        shape="circle" 
                        icon={this.state.audioLocked ? <LockOutlined /> : <UnlockOutlined />} 
                        onClick={this._lockSound.bind(this)}
                    />
                    <Slider 
                        ref={this._slider}
                        min={0} 
                        max={100}
                        onChange={this.onVolumeChange.bind(this)}
                        defaultValue={this.props.volume} 
                        disabled={this.state.audioLocked}
                        style={{width: '65%'}}
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
                this.props.onVolumeChange(this.props.title, this.props.deviceName, value, this.props.output, this.state.audio);
                console.log(this.props.title + ": " + value);
            }
        // We found that a good value of delay to have a gradual change of the volume is 150
        },150);
    }
}

export default VolumeBox;