import {Component, OnInit, OnDestroy} from '@angular/core';
import {Task} from '../../../common/models/task.model';
import {TasksService} from '../../services/tasks/tasks.service';
import {TaskStatus} from '../../../common/models/task.status.enum';
import {Preloader} from '../../../common/extensions/preloader/preloader.extension';
import {Observable, Subscription} from 'rxjs';
import {TasksBehavior} from '../configs';

@Component({
    selector: 'stash',
    styles: [require('./stash.scss')],
    template: require('./stash.template.html')
})
export class Stash implements OnInit, OnDestroy {
    private _stashInit: TasksBehavior = TasksBehavior.WITH_STORE;
    private _stashTasks: Array<Task> = [new Task({})]; //TODO: hack for displaying empty stash template
    private _taskText: string = '';
    private _stashTasks$: Observable<Task[]>;
    private _subscription: Subscription;
    private _interval: number;

    constructor(public tasksService: TasksService, private preloader: Preloader) {

    }

    public get stashTasks(): Array<Task>  {
        return this._stashTasks;
    }

    /**
     * Add stash task
     * unused now
     * @param taskText
     * @param tasks
     */
    public addStashTask(taskText: string, tasks: Task[]): void {
        let task: Task = new Task({text: taskText, isChecked: false, state: TaskStatus.COMPLITED});

        if (taskText) {
            this.tasksService.addTask(task)
                .subscribe(res => {
                    tasks.unshift(task);
                    this._taskText = '';
                });
        }
    }

    public removeStashTask(task: Task, tasks: Task[]): void {
        this.tasksService.removeTask(task.id)
            .subscribe(res => {
                let currentTaskIndex: number = tasks.indexOf(task);
                tasks.splice(currentTaskIndex, 1);
            });
    }

    public removeStashStoreTask(task: Task): void {
        this.tasksService.removeStoreTask(task.id);
    }

    public updateStashTask(task: Task, tasks: Task[]): void {
        let state: TaskStatus = TaskStatus.STASH;
        let currentTask = new Task(task);
        currentTask.state = state;

        this.tasksService.updateTask(task.id, currentTask)
            .subscribe(res => {
                task.state = state;
            });
    }

    public updateStashStoreTask(task: Task): void {
        let state: TaskStatus = TaskStatus.STASH;
        let currentTask = new Task(task);
        currentTask.state = state;

        this.tasksService.updateStoreTask(task.id, currentTask, state);
    }

    public unStashTask(task: Task, tasks: Task[]): void {
        let currentTask = new Task(task);
        currentTask.state = TaskStatus.NOT_COMPLITED;
        currentTask.orderNumber = this.tasksService.maxOrderNumber(tasks) + 1;

        this.tasksService.updateTask(task.id, currentTask)
            .subscribe((res) => {
                tasks.splice(tasks.indexOf(task), 1);
            });
    }

    public unStashStoreTask(task: Task): void {
        let currentTask = new Task(task);
        currentTask.state = TaskStatus.NOT_COMPLITED;
        currentTask.orderNumber = this.tasksService.maxOrderNumber(this.stashTasks) + 1;

        this.tasksService.updateStashStoreTask(task.id, currentTask);
    }

    public ngOnInit(): void {
        if (this._stashInit === TasksBehavior.WITH_STORE) {
            this.initializeStore();
        } else if (this._stashInit === TasksBehavior.WITHOUT_STORE) {
            this.initialize();
        } else {
            throw new Error('Stash component initialization error');
        }
    }

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
        this._stashTasks = null;
        window.clearInterval(this._interval);
    }

    private initialize(): void {
        this.tasksService.getTasks(TaskStatus.STASH)
            .subscribe(stashTasks => {
                this.preloader.hide();

                this._stashTasks = stashTasks;
            });
    }

    private initializeStore(): void {
        this._stashTasks$ = this.tasksService.tasks$; // TODO: need to add stash store

        this.tasksService.getStoreTask(TaskStatus.STASH);

        this._interval = window.setInterval(() => {
            this.tasksService.getStoreTask(TaskStatus.STASH);
        }, 10000);

        this._subscription = this._stashTasks$.subscribe(stashTasks => {
            this.preloader.hide();
            this._stashTasks = stashTasks;

            // console.log(this._stashTasks);
        });
    }
}
