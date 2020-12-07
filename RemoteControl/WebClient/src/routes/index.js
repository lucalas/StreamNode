import { Router } from 'preact-router';

import Profile from '../routes/profile';
import Dashboard from './dashboard';

const Routes = () => (
    <Router>
        <Dashboard path="/" />
        <Profile path="/profile/" user="me" />
        <Profile path="/profile/:user" />
    </Router>
)

export default Routes;