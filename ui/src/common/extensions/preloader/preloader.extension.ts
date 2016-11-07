import {Injectable} from '@angular/core';
import {PreloaderInterface} from './preloader.model';

@Injectable()
export class Preloader {
    private _preloader: PreloaderInterface = new PreloaderInterface();
    private _preloaderElement: HTMLElement = <HTMLElement>document.querySelector('#preloader');

    get isLoading() {
        return this._preloader.isLoading;
    }

    public show(): void {
        this._preloader.isLoading = true;

        this._preloaderElement.style.display = 'block';
    }

    public hide(): void {
        this._preloader.isLoading = false;

        setTimeout(() => { // TODO: looks like a hack. need to find a better way
            this._preloaderElement.style.display = 'none';
        }, 100);
    }
}
