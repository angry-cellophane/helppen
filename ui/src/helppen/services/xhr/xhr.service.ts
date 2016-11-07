import {Injectable} from '@angular/core';
import {BrowserXhr} from '@angular/http';

@Injectable()
export class CustomBrowserXhr extends BrowserXhr {
    constructor() {
        super();
    }
    build(): any {
        let xhr = super.build();
        xhr.origin = true;
        xhr.withCredentials = true;
        return <any>(xhr);
    }
}
