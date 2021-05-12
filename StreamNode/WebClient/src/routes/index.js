import { Router } from 'preact-router';

import Dashboard from './dashboard';

const Routes = ({language}) => (
    <Router>
        <Dashboard path="/" language={language}/>
    </Router>
)

export default Routes;