import {Component, OnInit, AfterViewInit} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {CORE_DIRECTIVES} from '@angular/common';
import {AuthService} from '../../services/auth';
import {Preloader} from '../../../common/extensions/preloader';
import {FormGroup, REACTIVE_FORM_DIRECTIVES, FORM_DIRECTIVES, FormBuilder, Validators} from '@angular/forms';

@Component({
    selector: 'login',
    directives: [RouterLink, CORE_DIRECTIVES, REACTIVE_FORM_DIRECTIVES, FORM_DIRECTIVES],
    styles: [require('./login.scss')],
    template: require('./login.template.html')
})
export class Login implements OnInit, AfterViewInit {
        public error: string;
        private _form: FormGroup;

    constructor(public router: Router, public formBuilder: FormBuilder, public auth: AuthService, private preloader: Preloader) {
        this._form = formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    public get form(): FormGroup {
        return this._form;
    }

    public login(form: any): void {
        this.auth.login(form.value.username, form.value.password)
            .subscribe(res => {
                this.router.navigate(['/tasks']);
                this.preloader.show();
            }, (err) => {
                this.error = err;
            });
    }

    public signup(event: Event): void {
        event.preventDefault();
        this.router.navigate(['/signup']);
    }

    public ngOnInit(): void {
        document.querySelector('body').classList.remove('colorful');
    }

    public ngAfterViewInit(): void {
        this.preloader.hide();
    }
}
