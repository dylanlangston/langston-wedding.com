import { WeddingEvent, Address } from './WeddingEvent';

export const weddingData: WeddingEvent = {
    title: 'Dylan & Mia\'s Wedding',
    description: 'Celebrate our special day with us!',
    date: new Date('2025-05-01T00:00:00'), 
    venue: {
        name: 'The Great Place',
        address: new Address('123 Main Street', 'City', 'State', '12345')
    }
};