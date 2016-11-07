import {enableProdMode, provide} from '@angular/core';
import {bootstrap} from '@angular/platform-browser-dynamic';
import {HashLocationStrategy, LocationStrategy} from '@angular/common';
import {HTTP_PROVIDERS, BrowserXhr, Http} from '@angular/http';
import {disableDeprecatedForms, provideForms} from '@angular/forms';
import {TRANSLATE_PROVIDERS, TranslateLoader, TranslateStaticLoader} from 'ng2-translate';

const ENV_PROVIDERS = [];
// depending on the app mode, enable prod mode or add debugging modules
if (PRODUCTION) {
    enableProdMode();
} else {
    ENV_PROVIDERS.push();
}

/*
 * App Component
 * our top level component that holds all of our components
 */
import {App} from './app';
import {CustomBrowserXhr} from './services/xhr/xhr.service';
import {Preloader} from '../common/extensions/preloader/preloader.extension';
import {APP_ROUTER_PROVIDERS} from './app/routes';

/*
 * Bootstrap our Angular app with a top level component `App` and inject
 * our Services and Providers into Angular's dependency injection
 */

document.addEventListener('DOMContentLoaded', function main() {
    return bootstrap(App, [
        // These are dependencies of our App
        HTTP_PROVIDERS,
        ENV_PROVIDERS,
        TRANSLATE_PROVIDERS,
        APP_ROUTER_PROVIDERS,

        provide(BrowserXhr, { useClass: CustomBrowserXhr }),
        provide(LocationStrategy, {useClass: HashLocationStrategy}), // use #/ routes, remove this for HTML5 mode
        provide(TranslateLoader, {
            useFactory: (http: Http) => new TranslateStaticLoader(http, '', '.json'),
            deps: [Http]
        }),
        disableDeprecatedForms(),
        provideForms(),

        Preloader,
    ])
        .catch(err => console.error(err));
});
