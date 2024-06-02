export interface WeddingEvent {
    title: string;
    description?: string;
    date: Date;
    venue: Venue;
}

export interface Venue {
    name: string;
    address: Address;
}

export class Address { 
    constructor(
        public readonly street: string,
        public readonly city: string,
        public readonly state: string,
        public readonly zip: string 
    ) {}

    // Example method to get a formatted address string
    getFullAddress(): string {
        return `${this.street}, ${this.city}, ${this.state} ${this.zip}`;
    }
}