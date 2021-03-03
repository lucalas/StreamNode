

import { Layout, Typography, Row, Col } from 'antd';

import CustomHeader from './customheader';
import Routes from '../routes';
import CustomSidebar from './sidebar';


const { Footer, Sider, Content } = Layout;
const {Title} = Typography;


const App = () => (
	<div id="app" class="bg-white">
		<Layout>
			<CustomSidebar/>
			<Layout>
				<CustomHeader />
				<Content>
					<Routes/>
				</Content>
				<Footer style={{backgroundColor: "#001529"}}>
					<Row justify="end">
						<Col><Title level={5} style={{color: "#fff"}}>v 1.0</Title></Col>
					</Row>
				</Footer>
			</Layout>
		</Layout>
	</div>
)

export default App;
