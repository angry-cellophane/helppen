interface Item {
    path: string;
    title: string;
    icon: string;
    detach?: boolean;
}

export class ProfileItem implements Item {
    public path: string;
    public title: string;
    public icon: string;
    public detach: boolean;

    constructor(options?: Item) {
        if (options) {
            Object.assign(this, options);
        }
    }
}
