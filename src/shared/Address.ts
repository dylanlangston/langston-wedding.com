export class Address { 
    constructor(
        public street: string,
        public city: string,
        public state: string,
        public zip: string 
    ) {}

    // Example method to get a formatted address string
    getFullAddress(): string {
        return `${this.street}, ${this.city}, ${this.state} ${this.zip}`;
    }
}