import {Component, AfterViewInit} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from '../../services/auth';
import {Preloader} from '../../../common/extensions/preloader/preloader.extension';

@Component({
    selector: 'logout',
    styles: [],
    template: ``
})
export class Logout implements AfterViewInit {
    constructor(public router: Router, private auth: AuthService, private preloader: Preloader) {

    }

    public ngAfterViewInit(): void {
        this.preloader.show();

        setTimeout(() => {
            this.auth.logout();
            this.router.navigate(['/login']);
            // location.reload();
        }, 1500);
    }
}
