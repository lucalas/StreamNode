import { Component } from 'preact';

import { Layout } from 'antd';
const { Header } = Layout;

import "./style.css"

class CustomHeader extends Component {

    constructor() {
        super();
    }

    render() {
        return (
            <Header className="header">
				<h1 className="header-title" style={{color: '#fff', textAlign: 'center', fontWeight:'bold'}}>DASHBOARD</h1>
			</Header>
		);
    }
};

export default CustomHeader;
