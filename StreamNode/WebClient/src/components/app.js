
import { useState } from 'preact/hooks'
import { Layout, Typography, Row, Col } from 'antd';

import CustomHeader from './customheader';
import Routes from '../routes';
import CustomSidebar from './sidebar';

import italy from  "../assets/flags/italy.svg"
import united_states from  "../assets/flags/united-states.svg"


const { Footer, Sider, Content } = Layout;
const { Title } = Typography;


const App = () => {

	const languages = [
		{
			id: "ita",
			img: italy 
		},
		{
			id: "eng",
			img: united_states
		}
	];
	const [language, setLanguage] = useState("ita");
	

	return (
		<div id="app" class="bg-white">
			<Layout>
				<CustomSidebar language={language}/>
				<Layout>
					<CustomHeader languages={languages} language={language} changeLanguage={setLanguage}/>
					<Content>
						<Routes language={language}/>
					</Content>
					<Footer style={{ backgroundColor: "#001529" }}>
						<Row justify="end">
							<Col><Title level={5} style={{ color: "#fff" }}>v 1.0</Title></Col>
						</Row>
					</Footer>
				</Layout>
			</Layout>
		</div>
	)
}

export default App;
