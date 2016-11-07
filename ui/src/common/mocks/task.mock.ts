import {Task} from '../models/task.model';
import {TaskStatus} from '../models/task.status.enum';

export const FIXTURE_TASKS: Array<Task> = [
    new Task({
        id: 1,
        creationDateTime: new Date(),
        lastChangeDateTime: new Date(),
        orderNumber: 1,
        ownerId: 12,
        state: TaskStatus.NOT_COMPLITED,
        text: 'text 1'
    }),
    new Task({
        id: 2,
        creationDateTime: new Date(),
        lastChangeDateTime: new Date(),
        orderNumber: 2,
        ownerId: 15,
        state: TaskStatus.NOT_COMPLITED,
        text: 'text 2'
    }),
    new Task({
        id: 3,
        creationDateTime: new Date(),
        lastChangeDateTime: new Date(),
        orderNumber: 3,
        ownerId: 27,
        state: TaskStatus.COMPLITED,
        text: 'text 3'
    })
];
