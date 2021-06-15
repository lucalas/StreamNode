import { Component, createRef } from 'preact';

import { useDrag, useDrop } from 'react-dnd'

import { Card, Slider, Button, Row, Col, Image, Avatar, Menu, Dropdown, Typography } from 'antd';
import { SoundOutlined, DragOutlined, AudioOutlined, AudioMutedOutlined, EllipsisOutlined } from '@ant-design/icons';

import HideIcon from '../../assets/images/hide-icon.svg';

const { Text, Paragraph } = Typography;

class VolumeBox extends Component {
    _slider = createRef();


    constructor(props) {
        super(props);
        this.state = {
            mute: this.props.defaultMute,
            audioLocked: false,
            micLocked: false
        }
    }

    _muteSound() {
        let newMuteValue = !this.state.mute;
        this.setState({
            mute: newMuteValue
        });
        this.props.onMutePressed(this.props.title, this.props.deviceName, this._slider.current.state.value, this.props.output, newMuteValue);
    }

    _lockSound() {
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
                Icon = <Image src={this.props.icon} width={this.props.isVertical ? 32 : 64} preview={false} style={{ filter: this.props.isEditable ? "grayscale(80%)" : "" }} />;
            } else {
                Icon = <Avatar size={this.props.isVertical ? 32 : 64} icon={<SoundOutlined />} />;
            }
        } else {
            if (this.state.mute) {
                Icon = <Avatar size={this.props.isVertical ? 32 : 64} icon={<AudioMutedOutlined />} />;
            } else {
                Icon = <Avatar size={this.props.isVertical ? 32 : 64} icon={<AudioOutlined />} />;
            }
        }
        return Icon;
    }

    setHideValue(value) {
        this.props.onHideEvent(value);
    }

    getMenu() {
        return (<Menu>
            <Menu.Item>
                <Text onClick={event => this.setHideValue(!this.props.isHide)}>{this.props.isHide ? "Show me" : "Hide me"}</Text>
            </Menu.Item>
        </Menu>)
    }

    render() {
        const [{ isDragging }, drag, dragPreview] = useDrag(() => ({
            type: 'BOX',
            item: {
                id: this.props.index
            },
            collect: (monitor) => ({
                item: monitor.getItem(),
                isDragging: monitor.isDragging()
            })
        }))

        const [{ isOver, item }, drop] = useDrop(() => ({
            accept: "BOX",
            drop: obj => this.props.dropEvent(obj.id, this.props.index),
            collect: monitor => ({
                isOver: !!monitor.isOver(),
                item: monitor.getDropResult()
            }),
        }))

        const extra = this.props.isEditable ? (
            <Dropdown overlay={this.getMenu()}>
                <EllipsisOutlined style={{fontSize:'150%'}} />
            </Dropdown>

        ) : null;

        const title = (
            <div>
                <Row>
                    <Col span={22}><Paragraph ellipsis>{this.props.title}</Paragraph></Col>
                    <Col span={2} hidden={!this.props.isEditable || !this.props.isHide}><div width="100%"><Image src={HideIcon} width={20} preview={false} /></div></Col>
                </Row>
                <Row><Text style={{ fontSize: 10 }}>{this.props.deviceName}</Text></Row>
            </div>);

        return (
            !this.props.isVertical

                ?

                // HORIZONTAL
                <div ref={dragPreview}>
                    <div ref={drop}>
                        <Card
                            title={title}
                            headStyle={{ backgroundColor: isOver ? 'rgba(24,144,255,0.5)' : this.props.isEditable ? "#ccc" : 'rgba(24,144,255,0.8)'}}
                            style={{ borderRight: '1px solid #f2f2f2', backgroundColor: isDragging ? 'rgba(24,144,255,0.5)' : "" }}
                            bordered={false}
                            extra={extra}
                        >
                            <Row justify="center">
                                {this.getAudioIcon()}
                            </Row>

                            <Col>
                                <Row justify="space-between" hidden={this.props.volumeHide}>
                                    {this.props.isEditable &&
                                        <Button
                                            ref={drag}
                                            icon={<DragOutlined />}
                                            style={{ cursor: "move" }}
                                            type="primary"
                                            shape="circle"
                                        />
                                    }

                                    <Button
                                        type={!this.state.mute ? "primary" : "default"}
                                        shape="circle"
                                        icon={this.getVolumeAudioIcon()}
                                        onClick={this._muteSound.bind(this)}
                                        disabled={this.props.isEditable}
                                    />

                                    <Slider
                                        className="slider-test-css-t"
                                        ref={this._slider}
                                        min={0}
                                        max={100}
                                        onChange={this.onVolumeChange?.bind(this)}
                                        defaultValue={this.props.volume}
                                        disabled={this.state.audioLocked || this.props.isEditable}
                                        style={{
                                            width: this.props.isEditable ? "65%" : "80%"
                                        }}
                                    />
                                </Row>
                            </Col>
                        </Card>
                    </div>
                </div>

                :

                //VERTICAL
                <div ref={dragPreview}>
                    <div ref={drop}>
                        <Card
                            title={
                                <Row justify="center">
                                    {this.getAudioIcon()}
                                </Row>
                            }
                            headStyle={{ textAlign: 'center', backgroundColor: isOver ? 'rgba(24,144,255,0.5)' : this.props.isEditable ? "#ccc" : 'rgba(24,144,255,0.8)', color: 'white' }}
                            style={{ borderRight: '1px solid #f2f2f2', backgroundColor: isDragging ? 'rgba(24,144,255,0.5)' : "" }}
                            bodyStyle={{ height: "auto", display: "flex", justifyContent: "center" }}
                            bordered={false}
                        >
                            <Col>
                                <Row style={{ margin: "10px 0" }}>
                                    <Slider vertical
                                        ref={this._slider}
                                        min={0}
                                        max={100}
                                        onChange={this.onVolumeChange.bind(this)}
                                        defaultValue={this.props.volume}
                                        disabled={this.state.audioLocked || this.props.isEditable}
                                        style={{ height: "200px" }}
                                    />
                                </Row>
                                <Row style={{ margin: "10px 0" }}>
                                    <Button
                                        type={!this.state.mute ? "primary" : "default"}
                                        shape="circle"
                                        icon={this.getVolumeAudioIcon()}
                                        onClick={this._muteSound.bind(this)}
                                        disabled={this.props.isEditable}
                                    />
                                </Row>
                                {this.props.isEditable &&
                                    <Row>
                                        <Button
                                            ref={drag}
                                            id="dragPoint"
                                            type="primary"
                                            shape="circle"
                                            icon={<DragOutlined />}
                                            style={{ cursor: isDragging ? "move" : "pointer" }}
                                        />
                                    </Row>
                                }
                            </Col>
                        </Card>
                    </div>
                </div>
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
        }, 150);
    }
}

export default VolumeBox;