

import { Layout } from 'antd';

import CustomHeader from './customheader';
import Routes from '../routes';
import CustomSidebar from './sidebar';


const { Footer, Sider, Content } = Layout;


const App = () => (
	<div id="app" class="bg-white">
		<Layout>
			<CustomSidebar/>
			<Layout>
				<CustomHeader />
				<Content>
					<Routes/>
				</Content>
				<Footer>Footer</Footer>
			</Layout>
		</Layout>
	</div>
)

export default App;
