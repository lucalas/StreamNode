import { Router } from 'preact-router';

import Dashboard from './dashboard';

const Routes = () => (
    <Router>
        <Dashboard path="/" />
    </Router>
)

export default Routes;