import {Component, OnInit} from '@angular/core';

@Component({
    selector: 'home',
    template: '<div>{{name}}</div>'
})
export class Home implements OnInit {
    public name: string = 'home';

    public ngOnInit(): void {

    }
}
