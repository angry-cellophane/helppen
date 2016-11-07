export const APP_NAME: string = 'HELPPEN';

export const LOCALE_FULL: string = 'ru-RU';
// export const LOCALE_FULL: string = 'en-EN';
export const LOCALE: string = LOCALE_FULL.substring(0, 2);
export const DEFAULT_LOCALE: string = LOCALE;

export enum TasksBehavior {
    WITHOUT_STORE,
    WITH_STORE
}
