import {Component, OnInit, OnDestroy} from '@angular/core';
import {TranslatePipe} from 'ng2-translate';
import {Task} from '../../../common/models/task.model';
import {TasksService} from '../../services/tasks/tasks.service';
import {TaskStatus} from '../../../common/models/task.status.enum';
import {Preloader} from '../../../common/extensions/preloader';
import {Observable, Subscription} from 'rxjs';
import {TasksBehavior} from '../configs';

@Component({
    selector: 'tasks',
    pipes: [TranslatePipe],
    styles: [require('./tasks.scss')],
    template: require('./tasks.template.html')
})
export class Tasks implements OnInit, OnDestroy {
    private _tasksInit: TasksBehavior = TasksBehavior.WITH_STORE;
    private _tasks: Array<Task> = [new Task({})]; //TODO: hack for displaying empty tasks template
    private _taskText: string = '';
    private _tasks$: Observable<Task[]>;
    private _subscription: Subscription;
    private _interval: number;

    constructor(private tasksService: TasksService, private preloader: Preloader) {

    }

    public get tasks(): Array<Task>  {
        return this._tasks;
    }

    public get taskText(): string {
        return this._taskText;
    }

    public set taskText(value: string) {
        this._taskText = value;
    }

    public addTask(taskText: string, tasks: Task[]): void {
        let task = new Task({text: taskText, isChecked: false});

        if (taskText) {
            this.tasksService.addTask(task)
                .subscribe(res => {
                    tasks.unshift(task);
                    this._taskText = '';
                });
        }
    }

    public addStoreTask(taskText: string): void {
        let task = new Task({text: taskText, isChecked: false});

        if (taskText) {
            this.tasksService.addStoreTask(task);
            this._taskText = '';
        }
    }

    public removeTask(task: Task, tasks: Task[]): void {
        this.tasksService.removeTask(task.id)
            .subscribe(res => {
                let currentTaskIndex: number = tasks.indexOf(task);
                tasks.splice(currentTaskIndex, 1);
            });
    }

    public removeStoreTask(task: Task): void {
        this.tasksService.removeStoreTask(task.id);
    }

    public updateTask(isChecked: boolean, task: Task, tasks: Task[]): void {
        let state: TaskStatus = isChecked ? TaskStatus.COMPLITED : TaskStatus.NOT_COMPLITED;
        let currentTask = new Task(task);
        currentTask.state = state;

        this.tasksService.updateTask(task.id, currentTask)
            .subscribe(res => {
                task.state = state;
            });
    }

    public updateStoreTask(isChecked: boolean, task: Task): void {
        let state: TaskStatus = isChecked ? TaskStatus.COMPLITED : TaskStatus.NOT_COMPLITED;
        let currentTask = new Task(task);
        currentTask.state = state;

        this.tasksService.updateStoreTask(task.id, currentTask, state);
    }

    public stashTask(task: Task, tasks: Task[]): void {
        let currentTask = new Task(task);
        currentTask.state = TaskStatus.STASH;

        this.tasksService.updateTask(task.id, currentTask)
            .subscribe((res) => {
                tasks.splice(tasks.indexOf(task), 1);
            });
    }

    public stashStoreTask(task: Task): void {
        let currentTask = new Task(task);
        currentTask.state = TaskStatus.STASH;

        this.tasksService.updateStashStoreTask(task.id, currentTask);
    }

    public moveTaskUp(task: Task, tasks: Task[]): void {
        let currentTask = new Task(task);

        currentTask.orderNumber = this.tasksService.maxOrderNumber(tasks) + 1;

        this.tasksService.updateTask(currentTask.id, currentTask)
            .subscribe(res => {
                tasks.unshift(currentTask);
                tasks.splice(tasks.indexOf(task), 1);
            });
    }

    public moveStoreTaskUp(task: Task): void {
        let currentTask = new Task(task);

        currentTask.orderNumber = this.tasksService.maxOrderNumber(this.tasks) + 1;

        this.tasksService.moveUpStoreTask(currentTask.id, currentTask);
    }

    public cleanStoreTask(taskText: string): void {
        this._taskText = '';
    }

    /**
     * Shift up task
     * unused now
     * @param {Task} task
     * @param {Task[]} tasks
     */
    public shiftTask(task: Task, tasks: Task[]): void {
        let currentTaskIndex: number = tasks.indexOf(task);
        if (currentTaskIndex !== 0) {
            tasks.splice( currentTaskIndex - 1, 0, tasks.splice(currentTaskIndex, 1)[0] );
        }
    }

    /**
     * Shift down task 
     * unused now
     * @param {Task} task
     * @param {Task[]} tasks
     */
    public unshiftTask(task: Task, tasks: Task[]): void {
        let currentTaskIndex: number = tasks.indexOf(task);
        if (currentTaskIndex !== tasks.length) {
            tasks.splice( currentTaskIndex + 1, 0, tasks.splice(currentTaskIndex, 1)[0] );
        }
    }

    public ngOnInit(): void {
        if (this._tasksInit === TasksBehavior.WITH_STORE) {
            this.initializeStore();
        } else if (this._tasksInit === TasksBehavior.WITHOUT_STORE) {
            this.initialize();
        } else {
            throw new Error('Tasks component initialization error');
        }
    }

    public ngOnDestroy(): void {
        this._subscription.unsubscribe();
        this._tasks = null;
        window.clearInterval(this._interval);
    }

    private initialize(): void {
        this.tasksService.getTasks()
            .subscribe(tasks => {
                this.preloader.hide();

                for (let i = 0; i < tasks.length; i++) {
                    tasks[i].isChecked = (tasks[i].state === TaskStatus.COMPLITED);
                }

                this._tasks = tasks;

            });
    }

    private initializeStore(): void {
        this._tasks$ = this.tasksService.tasks$;

        this.tasksService.getStoreTask();

        this._interval = window.setInterval(() => {
            this.tasksService.getStoreTask();
        }, 5000);

        this._subscription = this._tasks$.subscribe(tasks => {
            this.preloader.hide();

            for (let i = 0; i < tasks.length; i++) {
                tasks[i].isChecked = (tasks[i].state === TaskStatus.COMPLITED);
            }
            this._tasks = tasks;

            // console.log(this._tasks);
        });
    }
}
