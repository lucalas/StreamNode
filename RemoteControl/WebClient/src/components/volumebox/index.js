import { Component } from 'preact';

import { Card, Slider, Button } from 'antd';
import { SoundOutlined } from '@ant-design/icons';

class VolumeBox extends Component {

    props = {};

    constructor(props) {
        this.props = props;
    }
    
    render() {
        return (
            <Card title={this.props.title}>
                <Button type="primary" shape="circle" icon={<SoundOutlined />} />
                <Slider min={0} max={100} defaultValue={this.props.volume}/>
            </Card>
        );
    }
}

export default VolumeBox;