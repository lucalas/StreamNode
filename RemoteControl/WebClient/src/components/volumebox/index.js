import { Component } from 'preact';

import { Card, Slider, Button } from 'antd';
import { SoundOutlined } from '@ant-design/icons';

class VolumeBox extends Component {
    render() {
        return (
            <Card title="title">
                <Button type="primary" shape="circle" icon={<SoundOutlined />} />
                <Slider min={0} max={100} defaultValue={30}/>
            </Card>
        );
    }
}

export default VolumeBox;