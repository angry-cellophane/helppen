import {Injectable} from '@angular/core';
import {ApiToken} from '../api/api.token.service';
import {Observable} from 'rxjs/Rx';

@Injectable()
export class TokenService {
    constructor (public apiToken: ApiToken) {

    }

    public getToken(credentials: Object): Observable<any> {
        return this.apiToken.$getToken(credentials);
    }
}
