export class Api {
    public hostname: string = 'http://dev.helppen.com';
    public port: number = 80;
    public path: string = 'api';

    get root(): string {
        return `${this.hostname}:${this.port}/${this.path}`;
    }

    get host(): string {
        return `${this.hostname}:${this.port}`;
    }
}
