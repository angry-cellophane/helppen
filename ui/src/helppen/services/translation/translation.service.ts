import {Injectable} from '@angular/core';
import {TranslateService} from 'ng2-translate';
import {LOCALE, DEFAULT_LOCALE} from '../../app/configs';

@Injectable()
export class TranslationService {
    constructor(private translateService: TranslateService) {
        translateService.setDefaultLang(DEFAULT_LOCALE);
        translateService.use(LOCALE);
    }
}
