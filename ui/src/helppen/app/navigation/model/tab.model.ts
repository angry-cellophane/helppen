export class Tab {
    public path: string;
    public active: boolean;
    public icon: string;
    public title: string;

    constructor(options?: Tab) {
        if (options) {
            Object.assign(this, options);
        }
    }
}
