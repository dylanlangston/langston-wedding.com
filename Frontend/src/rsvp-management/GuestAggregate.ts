import { Guest } from "./Guest";
import { RSVP, RSVPStatus } from "./RSVP";

export interface GuestAggregate {
    getGuest(guestId: string): Promise<Guest | null>;
    getRSVPsForGuest(guestId: string): Promise<RSVP[]>;
    updateRSVPStatus(guestId: string, rsvpStatus: RSVPStatus): Promise<void>;
}