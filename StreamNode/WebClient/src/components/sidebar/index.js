import { Component } from 'preact';
import { Layout, Menu, Image, Row } from 'antd';
import { Link } from 'preact-router/match';
import { DesktopOutlined, BlockOutlined, CustomerServiceOutlined } from '@ant-design/icons';
import Logo from '../../assets/icons/logo.png';

//LANGUAGES
import languages from '../../data/languages.json'

const { Sider } = Layout;
class CustomSidebar extends Component {

    constructor() {
        super();
        this.state = {
            hideSidebar: false
        }
    }

    componentDidMount() {
        window.addEventListener("resize", this.hide.bind(this));
        this.hide();
    }
    
    hide() {
        this.setState({hideSidebar: window.innerWidth <= 480});
    }
    
    componentWillUnmount() {
        window.removeEventListener("hide", this.resize.bind(this));
    }

    render() {
        if(!this.state.hideSidebar) {
            return (
                <Sider collapsible defaultCollapsed>
                    <Row type="flex" justify="center" align="middle">
                        <Image src={Logo} width={70} preview={false} style={{padding:7}}/>
                    </Row>
                    <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline">
                        <Menu.Item key="1" icon={<DesktopOutlined />}>
                            <Link href="/">Dashboard</Link>
                        </Menu.Item>
                        <Menu.Item key="2" icon={<CustomerServiceOutlined />}>
                            <a href="#mixer">Mixer</a> {/*Con <Link> Non funziona */}
                        </Menu.Item>
                        <Menu.Item key="3" icon={<BlockOutlined />}>
                            <a href="#obs-scenes">{languages[this.props.language].sidebar.obs_scenes}</a> {/*Con <Link> Non funziona */}
                        </Menu.Item>
                    </Menu>
                </Sider>
                );
        }

        return null;
    }
}

export default CustomSidebar;