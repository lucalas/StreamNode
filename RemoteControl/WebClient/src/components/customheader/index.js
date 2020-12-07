import { Component } from 'preact';

import { Layout } from 'antd';
const { Header } = Layout;

class CustomHeader extends Component {

    constructor() {
        super();
    }

    render() {
        return (
			<Header>
				<h1>Preact App</h1>
			</Header>
		);
    }
};

export default CustomHeader;
