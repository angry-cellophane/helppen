import {Injectable} from '@angular/core';
import {Api} from '../../app/configs';
import {Http, Headers, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Task} from '../../../common/models/task.model';
import {TaskStatus} from '../../../common/models/task.status.enum';

@Injectable()
export class ApiTasks {

    constructor (private http: Http, private api: Api) {

    }

    get root(): string {
        return this.api.tasksUrl;
    }

    get headers(): Headers {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return headers;
    }

    public $getTasks(state?: TaskStatus): Observable<any> { //TODO: was Task[]
        return this.http.get(`${this.root}/tasks/`)
            .map((data: Response): any => {
                let body = data.json();

                if (state) {
                    return body.filter(item => {
                        return item.state === state;
                    });
                } else {
                    return body.filter(item => {
                        return item.state !== TaskStatus.STASH;
                    });
                }
            })
            .catch((err: any) => {
                console.error(err);
                return err;
            });
    }

    public $updateTask(taskId: number | string, task: Task): Observable<any> {
        return this.http.put(
            `${this.root}/tasks/${taskId}`,
            JSON.stringify(task),
            {headers: this.headers}
            );
    }

    public $removeTask(taskId: number | string): Observable<any> {
        return this.http.delete(`${this.root}/tasks/${taskId}`);
    }

    public $addTask(task: Task): Observable<any> {
        return this.http.post(
            `${this.root}/tasks`,
            JSON.stringify(task),
            {headers: this.headers}
        );
    }
}
