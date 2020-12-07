import { h } from 'preact';

import { Layout } from 'antd';


import CustomHeader from './customheader';
import Routes from '../routes';

const { Footer, Sider, Content } = Layout;

const App = () => (
	<div id="app" class="bg-white">
		<Layout>
			<Sider>Sider</Sider>
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
