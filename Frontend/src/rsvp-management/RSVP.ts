import { Guest } from "./Guest";

export interface IRSVP {
    guest: Guest;
    attending: boolean;
    plusOne?: string;
    dietaryRestrictions?: string;
    comment?: string;
    rsvpStatus: RSVPStatus;
}

export enum RSVPStatus {
    Attending = "Attending",
    NotAttending = "NotAttending",
}

export class RSVP implements IRSVP {
    constructor(
        public guest: Guest,
        public attending: boolean,
        public rsvpStatus: RSVPStatus,
        public plusOne?: string,
        public dietaryRestrictions?: string,
        public comment?: string
    ) { }
}
