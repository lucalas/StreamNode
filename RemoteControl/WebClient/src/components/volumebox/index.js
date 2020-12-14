import { Component } from 'preact';
import { useContext } from 'preact/hooks';

import { Card, Slider, Button } from 'antd';
import { SoundOutlined } from '@ant-design/icons';
import Context from '../../services/Context';

const context = useContext(Context);

class VolumeBox extends Component {
    render() {
        return (
            <Card title="title">
                <Button type="primary" shape="circle" icon={<SoundOutlined />} />
                <Slider min={0} max={100} defaultValue={30}/>
            </Card>
        );
    }

    getVolumes() {
        wssocket = context.wssocket;

    }
}

export default VolumeBox;