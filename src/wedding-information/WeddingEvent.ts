import { Entity } from "../shared/Entity";
import { IVenue, Venue } from "./Venue";

export interface IWeddingEvent {
    readonly id: string;
    title: string;
    description?: string;
    date: Date;
    venue: IVenue;
}

export class WeddingEvent extends Entity implements IWeddingEvent {
    constructor(
        public title: string,
        public date: Date,
        public venue: Venue,
        public description?: string,
    ) {
        super();
    }
}