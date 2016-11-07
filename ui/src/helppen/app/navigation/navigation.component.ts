import {Component} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';
import {CORE_DIRECTIVES} from '@angular/common';
import {TranslatePipe, TranslateService} from 'ng2-translate';
import {LOCALE} from '../configs';

import {Tab} from './model/tab.model';

@Component({
    selector: 'navigation',
    directives: [ROUTER_DIRECTIVES, CORE_DIRECTIVES],
    providers: [TranslatePipe],
    styles: [require('./navigation.scss')],
    template: `
        <div class="tabs">
            <ul class="tabs-list">
                <li class="tab-item" *ngFor="let tab of tabs">
                    <a [routerLink]="tab.path" role="tab" routerLinkActive="active" (click)="setTabActive(tab)">
                        <i class="fa {{tab.icon}}" aria-hidden="true"></i>&nbsp;
                        {{tab.title}}
                    </a>
                </li>
                
                <!--<li class="tab-options-item">
                    <a href="" role="tab" (click)="logout()"><i class="fa fa-cog" aria-hidden="true"></i></a>
                </li>-->
            </ul>
        </div>
    `
})
export class Navigation {
    public active: boolean = false;
    public tabs: Array<Tab>;

    constructor (private translateService: TranslateService) {
        this.translateService.get('NAVIGATION')
            .subscribe(translation => {
                this.setTabs(translation);
            });
    }

    public setTabActive(tab: Tab): void {
        this.tabs.map(tab => {
            tab.active = false;
        });
        tab.active = true;
    }

    private setTabs(translation: any): void {
        this.tabs = [
            new Tab({
                path: 'tasks',
                active: false,
                icon: 'fa-check-circle',
                title: translation.TASKS._
            }),
            new Tab({
                path: 'stash',
                active: false,
                icon: 'fa-stack-overflow',
                title: translation.STASH._
            })
        ];
    }
}
