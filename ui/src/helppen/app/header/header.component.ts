import {Component, OnInit, ElementRef, Renderer, OnDestroy} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';
import {TranslatePipe, TranslateService} from 'ng2-translate';
import {ProfileItem} from './model/profile-item.model';
import {LOCALE} from '../configs/config';
import {Cookie} from '../../../common/extensions/cookie/cookie.extension';

@Component({
    selector: 'header',
    directives: [ROUTER_DIRECTIVES],
    providers: [TranslatePipe],
    styles: [require('./header.scss')],
    template: `
        <div id="header-area">
            <div id="brand-area">
                help pen
            </div>
            
            <div id="profile-area">
                <span class="profile-username">{{username}}</span>
                
                <span class="profile-avatar">
                    <i class="fa fa-user profile-icon" aria-hidden="true"></i>
                </span>
                
                <ul class="profile-menu">
                    <li *ngFor="let profileItem of profileItems" [class.detach]="profileItem.detach">
                        <a [routerLink]="profileItem.path">
                            <i class="fa {{profileItem.icon}}" aria-hidden="true"></i>
                            {{profileItem.title}}
                        </a>
                    </li>
                </ul>
            </div>
            
        </div>
    `
})
export class Header implements OnInit, OnDestroy {
    public username: string;
    private _profileMenuElement: HTMLElement;
    private _usernameElement: HTMLElement;
    private _avatarElement: HTMLElement;
    private _bodyListen: Function;
    private _usernameListen: Function;
    private _avatarListen: Function;
    private _profileItems: Array<ProfileItem>;

    constructor(private elementRef: ElementRef, private renderer: Renderer, private translateService: TranslateService) {
        this.translateService.get('PROFILE')
            .subscribe(translation => {
                this.setProfileItems(translation);
            });
    }

    public get profileItems(): Array<ProfileItem> {
        return this._profileItems;
    }

    public ngOnInit(): void {
        if (!!Cookie.getCookie('authName')) {
            this.username = Cookie.getCookie('authName');
        }

        this._profileMenuElement = this.elementRef.nativeElement.querySelector('.profile-menu');
        this._usernameElement = this.elementRef.nativeElement.querySelector('.profile-username');
        this._avatarElement = this.elementRef.nativeElement.querySelector('.profile-avatar');

        this._usernameListen = this.renderer.listen(this._usernameElement, 'click', this.onToggleOpened.bind(this));
        this._avatarListen = this.renderer.listen(this._avatarElement, 'click', this.onToggleOpened.bind(this));
    }

    public ngOnDestroy(): void {
        this._bodyListen();
        this._usernameListen();
        this._avatarListen();
    }

    private setProfileItems(translation: any) {
        this._profileItems = [
            new ProfileItem({
                path: 'settings',
                title: translation.SETTINGS,
                icon: 'fa-cog'
            }),
            new ProfileItem({
                path: 'logout',
                title: translation.LOGOUT,
                icon: 'fa-sign-out',
                detach: true
            })
        ];
    }

    private onToggleOpened(event: MouseEvent): void {
        if (this._profileMenuElement.classList.contains('profile-opened')) {
            this._profileMenuElement.classList.remove('profile-opened');
        } else {
            this._bodyListen = this.renderer.listen(document.body, 'click', this.onToggleClosed.bind(this));
            this._profileMenuElement.classList.add('profile-opened');
        }
    }

    private onToggleClosed(event: MouseEvent): void {
        // console.log('body event', event);

        if (!(<HTMLElement>event.target).classList.contains('profile-username') && !(<HTMLElement>event.target).classList.contains('profile-icon')) {
            this._bodyListen();
            this._profileMenuElement.classList.remove('profile-opened');
        }
    }
}
