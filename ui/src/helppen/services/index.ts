import {AuthManager} from './auth-manager';
import {AuthService} from './auth';
import {TokenService} from './token';
import {ApiToken} from './api';

export {AuthService} from './auth';
export {AuthManager} from './auth-manager';
export {TokenService} from './token';
export {ApiToken} from './api';

export const AUTH_PROVIDERS: any[] = [
    AuthManager,
    AuthService,
    TokenService,
    ApiToken
];
