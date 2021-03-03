import { Component } from 'preact';
import { Layout, Menu } from 'antd';
import { Link } from 'preact-router/match';
import { DesktopOutlined, BlockOutlined, CustomerServiceOutlined } from '@ant-design/icons';

const { Sider } = Layout;
class CustomSidebar extends Component {

    constructor() {
        super();
    }

    render() {
        return (
        <Sider collapsible defaultCollapsed>
            <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline">
                <Menu.Item key="1" icon={<DesktopOutlined />}>
                    <Link href="/">Dashboard</Link>
                </Menu.Item>
                <Menu.Item key="2" icon={<CustomerServiceOutlined />}>
                    <a href="#mixer">Mixer</a> {/*Con <Link> Non funziona */}
                </Menu.Item>
                <Menu.Item key="3" icon={<BlockOutlined />}>
                    <a href="#obs-scenes">OBS Scenes</a> {/*Con <Link> Non funziona */}
                </Menu.Item>
            </Menu>
        </Sider>
        );
    }
};

export default CustomSidebar;