import {RouterConfig, provideRouter} from '@angular/router';
import {AuthManager} from '../services/auth-manager';
import {AuthService} from '../services/auth';
import {TokenService} from '../services/token';
import {ApiToken} from '../services/api';

import {Header} from './header';
import {Navigation} from './navigation';

import {Home} from './home';
import {Tasks} from './tasks';
import {Stash} from './stash';
import {Login} from './login';
import {Logout} from './logout';
import {Settings} from './settings';

export const APP_DIRECTIVES: any[] = [
    Header,
    Navigation
];

export const APP_ROUTES: RouterConfig = [
    {
        path: 'home',
        component: Home,
        canActivate: [AuthManager]
    },
    {
        path: 'tasks',
        component: Tasks,
        canActivate: [AuthManager]
    },
    {
        path: 'stash',
        component: Stash,
        canActivate: [AuthManager]
    },
    {
        path: 'settings',
        component: Settings,
        canActivate: [AuthManager]
    },
    {
        path: 'login',
        component: Login,
    },
    {
        path: 'logout',
        component: Logout
    },
    {
        path: '',
        redirectTo: 'tasks',
        pathMatch: 'full'
    },
    {
        path: '**',
        redirectTo: 'tasks',
    }
];

export const AUTH_PROVIDERS = [
    AuthManager,
    AuthService,
    TokenService,
    ApiToken

];

export const APP_ROUTER_PROVIDERS = [
    provideRouter(APP_ROUTES),
    AUTH_PROVIDERS
];
