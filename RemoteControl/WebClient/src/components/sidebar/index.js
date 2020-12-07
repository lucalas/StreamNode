import { Component } from 'preact';
import { Layout, Menu } from 'antd';
import { Link } from 'preact-router/match';
import { PieChartOutlined } from '@ant-design/icons';

const { Sider } = Layout;
class CustomSidebar extends Component {

    constructor() {
        super();
    }

    render() {
        return (
        <Sider collapsible>
            <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline">
                <Menu.Item key="1" icon={<PieChartOutlined />}>
                    <Link href="/">Dashboard</Link>
                </Menu.Item>
            </Menu>
        </Sider>
        );
    }
};

export default CustomSidebar;