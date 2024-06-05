import { Address } from '../shared/Address';
import { Venue } from './Venue';
import { WeddingEvent } from './WeddingEvent';

export const weddingData = new WeddingEvent(
    'Dylan & Mia\'s Wedding',
    new Date('2025-05-01T00:00:00'), 
    new Venue(
        'The Great Place',
        new Address('123 Main Street', 'City', 'State', '12345'),
        new URL('https://example.com')
    ),
    'Celebrate our special day with us!'
);