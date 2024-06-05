import { Address } from "../shared/Address";
import { Entity } from "../shared/Entity";

export interface IVenue {
    readonly id: string;
    name: string;
    address: Address;
    link: URL;
}


export class Venue extends Entity implements IVenue {
    constructor(
        public name: string,
        public address: Address,
        public link: URL
    ) {
        super();
    }
}