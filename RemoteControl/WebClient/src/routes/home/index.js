import { h } from 'preact';
import style from './style.css';
import { Button, Tooltip } from 'antd';
import { SearchOutlined } from '@ant-design/icons';

class Home extends Component {

    constructor() {
        super();
    }

    render() {
		return (
		<div class={style.home}>
			<h2>Home</h2>
			<p>This is the Home component.</p>
			<Tooltip title="search">
				<Button type="primary" shape="circle" icon={<SearchOutlined />} />
			</Tooltip>
		</div>
		);
    }
};

export default Home;
