import { Address } from "../shared/Address";
import { Entity } from "../shared/Entity";

export interface IGuest {
    readonly id: string;
    name: string;
    email: string;
    phoneNumber?: string;
    address?: Address;
}

export class Guest extends Entity implements IGuest {
    constructor(
        public name: string,
        public email: string,
        public phoneNumber?: string,
        public address?: Address,
    ) {
        super();
     }
}