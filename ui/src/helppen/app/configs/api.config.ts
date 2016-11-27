import {Injectable} from '@angular/core';

@Injectable()
export class Api {

    get tasksUrl(): string {
    	return 'http://' + window.location.host + '/api';
    }

    get authUrl(): string {
    	return 'http://' + window.location.host + '/auth';
    }
}
