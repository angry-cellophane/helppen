import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Rx';
import {Subject} from 'rxjs/Subject';
import {ApiTasks} from '../api/api.tasks.service';
import {Task} from '../../../common/models/task.model';
import {TaskStatus} from '../../../common/models/task.status.enum';

@Injectable()
export class TasksService {
    private _tasks$: Subject<Task[]>;
    private _stash$: Subject<Task[]>;
    private _tasksStore: {tasks: Task[], stash: Task[]};

    constructor (public apiTasks: ApiTasks) {
        this._tasks$ = <Subject<Task[]>>new Subject();
        this._stash$ = <Subject<Task[]>>new Subject();
        this._tasksStore = {tasks: [], stash: []};
    }

    public get tasks$(): Observable<Task[]> {
        return this._tasks$.asObservable();
    }

    public get stash$(): Subject<Task[]> {
        return this._stash$;
    }

    public getTasks(state?: TaskStatus): Observable<Task[]> {
        return this.apiTasks.$getTasks(state);
    }

    public getStoreTask(state?: TaskStatus): void {
        this.apiTasks.$getTasks(state)
            .subscribe(res => {
                this._tasksStore.tasks = res;
                this._tasks$.next(this._tasksStore.tasks);
            });
    }

    public updateTask(taskId: number | string, task: Task): Observable<any> {
        return this.apiTasks.$updateTask(taskId, task);
    }

    public updateStoreTask(taskId: number | string, task: Task, state: TaskStatus): void {
        this.apiTasks.$updateTask(taskId, task)
            .subscribe(res => {
            for (let i = 0; i < this._tasksStore.tasks.length; i++) {
                if (this._tasksStore.tasks[i].id === taskId) {
                    this._tasksStore.tasks[i].state = state;
                }
            }
            this._tasks$.next(this._tasksStore.tasks);
        });
    }

    public updateStashStoreTask(taskId: number | string, task: Task): void {
        this.apiTasks.$updateTask(taskId, task)
            .subscribe(res => {
                for (let i = 0; i < this._tasksStore.tasks.length; i++) {
                    if (this._tasksStore.tasks[i].id === taskId) {
                        this._tasksStore.tasks.splice(i, 1);
                    }
                }

                this._tasks$.next(this._tasksStore.tasks);
            });
    }

    public moveUpStoreTask(taskId: number | string, task: Task): void {
        this.apiTasks.$updateTask(taskId, task)
            .subscribe(res => {
                for (let i = 0; i < this._tasksStore.tasks.length; i++) {
                    if (this._tasksStore.tasks[i].id === taskId) {
                        this._tasksStore.tasks.splice(i, 1);
                    }
                }
                this._tasksStore.tasks.unshift(task);

                this._tasks$.next(this._tasksStore.tasks);
            });
    }

    public removeTask(taskId: number | string): Observable<any> {
        return this.apiTasks.$removeTask(taskId);
    }

    public removeStoreTask(taskId: number | string): void {
        this.apiTasks.$removeTask(taskId)
            .subscribe(res => {
                for (let i = 0; i < this._tasksStore.tasks.length; i++) {
                    if (this._tasksStore.tasks[i].id === taskId) {
                        let currentTaskIndex: number = this._tasksStore.tasks.indexOf(this._tasksStore.tasks[i]);
                        this._tasksStore.tasks.splice(currentTaskIndex, 1);
                    }
                }
                this._tasks$.next(this._tasksStore.tasks);

            });
    }

    public addTask(task: Task): Observable<Task> {
        return this.apiTasks.$addTask(task);
    }

    public addStoreTask(task: Task): void {
        this.apiTasks.$addTask(task)
            .subscribe(res => {
                this._tasksStore.tasks.unshift(res.json());
                this._tasks$.next(this._tasksStore.tasks);
            });
    }

    public maxOrderNumber(tasks: Task[]): number {
        if (tasks instanceof Array) {
            if (tasks.length < 1) {
                return 0;
            } else {
                return tasks[0].orderNumber;
            }
        } else {
            return 0;
        }
    }
}
