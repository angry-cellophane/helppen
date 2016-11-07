import {Directive, Attribute, ViewContainerRef, DynamicComponentLoader} from '@angular/core';
import {Router, RouterOutlet, ComponentInstruction} from '@angular/router-deprecated';
import {Cookie} from '../../../common/extensions/cookie/cookie.extension';

@Directive({
    selector: 'secure-outlet',
})
export class SecureOutlet extends RouterOutlet {
    publicRoutes: any;
    private parentRouter: Router;

    constructor(
        _viewContainerRef: ViewContainerRef,
        _loader: DynamicComponentLoader,
        _parentRouter: Router,
        @Attribute('name') nameAttr: string
    ) {
        super(_viewContainerRef, _loader, _parentRouter, nameAttr);

        this.parentRouter = _parentRouter;

        this.publicRoutes = {
            'login': true,
            'signup': true
        };

    }

    public activate(instruction: ComponentInstruction): any {
        let url = instruction.urlPath;

        if (!this.publicRoutes[url] && !Cookie.getCookie('authToken')) {
            this.parentRouter.navigateByUrl('/login');
        }

        return super.activate(instruction);
    }
}
