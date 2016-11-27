import {Injectable} from '@angular/core';

@Injectable()
export class Api {

    get root(): string {
        return `${this.host}/api`;
    }

    get host(): string {
        return window.location.host;
    }
}
