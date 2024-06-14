import { RSVP } from "./RSVP";

export interface IRSVPService {
    getRSVPs(): Promise<RSVP[]>;
    createRSVP(rsvp: RSVP): Promise<RSVP>;
    updateRSVP(rsvp: RSVP): Promise<RSVP>;
}