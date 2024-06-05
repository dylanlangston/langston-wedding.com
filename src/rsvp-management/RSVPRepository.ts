import { RSVP } from "./RSVP";

export interface IRSVPRepository {
    getRSVPs(): Promise<RSVP[]>;
    saveRSVP(rsvp: RSVP): Promise<RSVP>;
    deleteRSVP(rsvpId: string): Promise<void>;
}