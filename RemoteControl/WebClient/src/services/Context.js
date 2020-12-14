import { createContext } from 'preact';
import WSSocketConnector from './wssocketconnector';

const Context = createContext({wssocket: WSSocketConnector });
export default Context;