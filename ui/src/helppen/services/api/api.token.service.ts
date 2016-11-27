import {Injectable} from '@angular/core';
import {Api} from '../../app/configs';
import {Http, RequestOptions, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';

@Injectable()
export class ApiToken {
    constructor (private http: Http, private api: Api) {

    }

    public $getToken(credentials: Object): Observable<any> {
        let body = JSON.stringify(credentials);
        let headers = new Headers({'Content-Type': 'application/json'});
        let options = new RequestOptions({headers: headers});

        return this.http.post(`${this.api.host}/auth/token`, body, options)
            .map<any>((data: any) => {
                return data;
            })
            .catch((err: any) => {
                if (err.status === 400) {
                    return Observable.throw(err._body);
                } else {
                    return Observable.throw('Server error');
                }
            });
    }
}
