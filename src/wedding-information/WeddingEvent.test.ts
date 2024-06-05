import { describe, expect, it } from 'vitest';
import { IWeddingEvent } from './WeddingEvent';
import { weddingData } from './WeddingData';
import { Address } from '../shared/Address';

describe('WeddingEvent', () => {
    it('should create a WeddingEvent with the correct properties', () => {
        const weddingEvent: IWeddingEvent = {
            id: '1-2-3-4-5',
            title: 'Dylan & Mia\'s Wedding',
            description: 'Celebrate our special day with us!',
            date: new Date('2025-05-01T00:00:00'),
            venue: {
                id: '1-2-3-4-5',
                name: 'The Great Place',
                address: new Address('123 Main Street', 'City', 'State', '12345'),
                link: new URL('https://example.com')
            }
        };

        expect(weddingEvent.title).toBe(weddingData.title);
        expect(weddingEvent.description).toBe(weddingData.description);
        expect(weddingEvent.date).toEqual(weddingData.date);
        expect(weddingEvent.venue.name).toBe(weddingData.venue.name);
        expect(weddingEvent.venue.address.street).toBe(weddingData.venue.address.street);
        expect(weddingEvent.venue.address.city).toBe(weddingData.venue.address.city);
        expect(weddingEvent.venue.address.state).toBe(weddingData.venue.address.state);
    });
});