import {Component, OnInit, OnDestroy} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';
import {APP_DIRECTIVES} from '../routes';

import {SecureOutlet} from '../../providers';
import {AuthService} from '../../services/auth';
import {Subscription} from 'rxjs';

// import {Navigation} from '../navigation';
// import {Header} from '../header';

@Component({
    selector: 'contents',
    directives: [ROUTER_DIRECTIVES, /*SecureOutlet,*/ APP_DIRECTIVES],
    // providers: [AuthService],
    styles: [require('./contents.scss')],
    template: `
        <header *ngIf="isLoggedIn"></header>
        
        <div id="contents-area" [ngClass]="{'contents-area': isLoggedIn}">
            <navigation *ngIf="isLoggedIn"></navigation>
            
            <div id="contents-outlet" [ngClass]="{'contents-outlet': isLoggedIn}">
                <!--<secure-outlet></secure-outlet>-->
                <router-outlet></router-outlet>
            </div>
        </div>
        
        <p class="copyright" [class.white]="isLoggedIn">© HELP PEN 2016</p>
        `
})
export class Contents implements OnInit, OnDestroy {
    public name: string = 'contents';
    private _isLoggedIn: boolean;
    private _loggedInSubscription: Subscription;

    constructor(private authService: AuthService) {

        this._loggedInSubscription = authService.loggedIn$.subscribe((res) => {
            this._isLoggedIn = res;

            if (this._isLoggedIn) {
                document.querySelector('body').classList.add('colorful');
            } else {
                document.querySelector('body').classList.remove('colorful');
            }
        });
    }

    public get isLoggedIn(): boolean {
        return this._isLoggedIn;
    }

    public ngOnInit(): void {
        document.querySelector('body').classList.add('colorful');

        this._isLoggedIn = this.authService.loggedIn;
    }

    public ngOnDestroy(): void {
        this._loggedInSubscription.unsubscribe();
    }
}
