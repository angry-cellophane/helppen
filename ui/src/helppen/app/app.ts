import {Component} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import '../../assets/styles/main.scss';

import {Api} from './configs';

import {Contents} from'./contents';
import {TasksService} from '../services/tasks/tasks.service';
import {ApiTasks} from '../services/api/api.tasks.service';
import {TranslationService} from '../services/translation';
// import {AUTH_PROVIDERS} from '../services';

@Component({
    selector: 'app',
    providers: [TranslationService, TasksService, ApiTasks],
    directives: [ROUTER_DIRECTIVES, Contents],
    pipes: [],
    styles: [],
    template: `
        <contents></contents>
    `
})

export class App {
    public appName: string = 'helppen';

    private api: any = new Api();

    constructor(private translationService: TranslationService) {

    }
}
