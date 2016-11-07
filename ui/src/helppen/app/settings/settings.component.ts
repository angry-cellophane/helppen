import {Component, OnInit} from '@angular/core';
import {Preloader} from '../../../common/extensions/preloader';

@Component({
    selector: 'settings',
    styles: [],
    template: `Настройки`
})
export class Settings implements OnInit {
    constructor(private preloader: Preloader) {

    }

    public ngOnInit(): void {
        this.preloader.hide();
    }
}
