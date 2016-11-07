import {Injectable} from '@angular/core';

@Injectable()
export class Cookie {
    static setCookie(name, value): void {
        document.cookie = `${name}=${value}`;
    }

    static getCookie(name): string {
        // let regex = new RegExp(`(?:(?:^|.*;\s*)${name}\s*\=\s*([^;]*).*$)|^.*$`);
        // return document.cookie.replace(regex, '$1');

        var matches = document.cookie.match(new RegExp(
            '(?:^|; )' + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + '=([^;]*)'
        ));
        return matches ? decodeURIComponent(matches[1]) : undefined;
    }

    static removeCookie(name): void {
        document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC`;
    }
}
