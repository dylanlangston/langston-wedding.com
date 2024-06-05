import { v4 as uuidv4 } from 'uuid';

export class Entity {
    public get id(): string {
        return this._id;
    }
    private readonly _id: string;

    constructor() {
        this._id = uuidv4();
    }
}