import { Component } from 'preact';

import { Card, Slider, Button, Row, Avatar, Col } from 'antd';
import { SoundOutlined, LockOutlined, UnlockOutlined, AudioOutlined, AudioMutedOutlined } from '@ant-design/icons';


class VolumeBox extends Component {
    constructor(props) {
        super(props);
        this.state = {
            audio: false,
            audioLocked: false,
            mic: false,
            micLocked: false
        }
    }

    _muteSound(){
        this.setState({
            audio: !this.state.audio
        });
    }

    _muteMic(){
        this.setState({
            mic: !this.state.mic
        });
    }

    _lockMic(){
        this.setState({
            micLocked: !this.state.micLocked
        });
    }

    _lockSound(){
        this.setState({
            audioLocked: !this.state.audioLocked
        });
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
                <Row justify="space-between" style={{marginBottom: 5}}>
                    <Button 
                        type={this.state.audio ? "default" : "primary"}
                        shape="circle" 
                        icon={<SoundOutlined/>} 
                        onClick={() => this._muteSound()}
                    />
                    <Button 
                        type={this.state.audioLocked ? "primary" : "default"}
                        shape="circle" 
                        icon={this.state.audioLocked ? <LockOutlined /> : <UnlockOutlined />} 
                        onClick={() => this._lockSound()}
                    />
                    <Slider 
                        min={0} 
                        max={100}
                        onChange={this.onVolumeChange.bind(this)}
                        defaultValue={this.props.volume} 
                        disabled={this.state.audioLocked}
                        style={{width: '65%'}}
                    />
                </Row>
                <Row justify="space-between">
                    <Button 
                        type={this.state.mic ? "default" : "primary"}
                        shape="circle" 
                        icon={!this.state.mic ? <AudioOutlined /> : <AudioMutedOutlined />} 
                        onClick={() => this._muteMic()}
                    />
                    <Button 
                        type={this.state.micLocked ? "primary" : "default"}
                        shape="circle" 
                        icon={this.state.micLocked ? <LockOutlined /> : <UnlockOutlined />} 
                        onClick={() => this._lockMic()}
                    />
                    <Slider 
                        min={0} 
                        max={100}
                        // FIXME change volumechanger for microphone
                        onChange={this.onVolumeChange.bind(this)}
                        defaultValue={this.props.volume} 
                        disabled={this.state.micLocked}
                        style={{width: '65%'}}
                    />
                </Row>
                </Col>
            </Card>
        );
    }

    onVolumeChange(value) {
        console.log("volume changed");
        if (this.props.onVolumeChange !== undefined) {
            this.props.onVolumeChange(this.props.title, this.props.deviceName, value);
        }
    }
}

export default VolumeBox;