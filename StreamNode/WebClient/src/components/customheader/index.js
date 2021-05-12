import { Component } from 'preact';

import { Layout } from 'antd';
import {CaretDownOutlined} from '@ant-design/icons';
const { Header } = Layout;

import "./style.css"

class CustomHeader extends Component {

    constructor() {
        super();
        this.state = {
            flagsOpened: false            
        }
    }

    openFlags(){
        this.setState({
            flagsOpened: !this.state.flagsOpened
        })
    }

    render() {
        return (
            <Header className="header" style={{height: "auto"}}>
                <div style={{display: "flex", flexDirection:"row", justifyContent: "center", padding: "0 20px"}}>
				    <h1 className="header-title" style={{color: '#fff', textAlign: 'center', fontWeight:'bold'}}>DASHBOARD</h1>
                    
                    <div style={{
                        display: "flex", 
                        flexDirection:"column", 
                        justifyContent:'center', 
                        cursor: "pointer", 
                        position: 'absolute', 
                        right: "36px", 
                        top: "18px",
                        backgroundColor: "rgba(238, 238, 238, 0.5)",
                        borderRadius: "10px",
                        padding: "5px 10px"
                    }} onClick={()=>this.openFlags()}>
                        <span style={{display: "flex", flexDirection:"row", alignItems: 'center', width:"42px", justifyContent: "space-between"}}>
                            <img src={this.props.languages.filter(el => el.id === this.props.language)[0].img} width="24" style={{height: "auto"}}/>
                            <CaretDownOutlined style={{color: "white"}}/>
                        </span>
                        {
                            this.state.flagsOpened &&
                            this.props.languages.map((el) => 
                                el.id !== this.props.language ? 
                                    <img 
                                        src={el.img} 
                                        style={{width:"24px", height:"auto"}}
                                        onClick={()=>this.props.changeLanguage(el.id)}
                                    /> 
                                    : 
                                    null
                            )
                        }
                    </div>
                </div>
            </Header>
		);
    }
};

export default CustomHeader;
