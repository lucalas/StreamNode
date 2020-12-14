

import { Layout } from 'antd';

import CustomHeader from './customheader';
import Routes from '../routes';
import CustomSidebar from './sidebar';
import Context from '../services/Context';


const { Footer, Sider, Content } = Layout;

const App = () => (
	<Context.Provider>
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
	</Context.Provider>
)

export default App;
