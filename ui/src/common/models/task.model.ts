import {TaskStatus} from './task.status.enum';

export class Task {
    public id: number | string;
    public creationDateTime: Date;
    public lastChangeDateTime: Date;
    public orderNumber: number;
    public ownerId: number;
    public state: TaskStatus;
    public isChecked: boolean;
    public text: string;

    constructor(
        options: {
            id?: number | string
            creationDateTime?: Date,
            lastChangeDateTime?: Date,
            orderNumber?: number,
            ownerId?: number,
            state?: TaskStatus,
            isChecked?: boolean,
            text?: string
        }
    ) {
        this.id = options.id;
        this.creationDateTime = options.creationDateTime;
        this.lastChangeDateTime = options.lastChangeDateTime;
        this.orderNumber = options.orderNumber;
        this.ownerId = options.ownerId;
        this.state = options.state;
        this.isChecked = this.state === TaskStatus.COMPLITED;

        this.text = options.text;
    }
}
