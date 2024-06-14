import { v4 as uuidv4 } from 'uuid';

// This goes against the principle of ubiqitous language
// We should consider removing this perhaps
export abstract class Entity {
    public readonly id: string;
    constructor() {
        this.id = uuidv4();
    }
}