import {Injectable, EventEmitter} from '@angular/core';
import {Observable, Observer, Subject} from 'rxjs/Rx';
import {TokenService} from '../token/token.service';
import {Cookie} from '../../../common/extensions/cookie/cookie.extension';

@Injectable()
export class AuthService {
    public loggedIn: boolean = false;
    private _loggedIn$: Observable<any>;
    private _loggedInSource: Subject<boolean> = new Subject<boolean>();

    constructor (private tokenService: TokenService) {
        this.loggedIn = !!Cookie.getCookie('authToken');
        this._loggedIn$ = this._loggedInSource.asObservable();
    }

    public get loggedIn$(): Observable<any> {
        return this._loggedIn$;
    }

    public emitLoggedIn(value?: boolean): void {
        this._loggedInSource.next(value ? value : this.loggedIn);
    }

    public login(login: string, password: string): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.tokenService.getToken({login, password})
                .subscribe((res) => {
                    if (res.status === 200) {
                        Cookie.setCookie('authToken', res.json().token);
                        Cookie.setCookie('authName', res.json().username);

                        this.loggedIn = true;
                        this.emitLoggedIn();

                        observer.next({});
                        observer.complete();
                    } else {
                        observer.error(res);
                    }
                }, (err) => {
                    observer.error(err);
                });
        });
    }

    public logout(): void {
        Cookie.removeCookie('authToken');
        Cookie.removeCookie('authName');
        this.loggedIn = false;
        this.emitLoggedIn();
    }
}
