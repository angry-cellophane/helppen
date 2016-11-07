export class Api {
    public hostname: string = 'http://localhost';
    public port: number = 8000;
    public path: string = 'api';

    get root(): string {
        return `${this.hostname}:${this.port}/${this.path}`;
    }

    get host(): string {
        return `${this.hostname}:${this.port}`;
    }
}
