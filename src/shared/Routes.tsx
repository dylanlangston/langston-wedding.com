import { ReactElement } from "react";
import { EditCalendar, People, Phone, LocationOn, Info, Redeem } from '@mui/icons-material';
import { weddingData } from '../wedding-information/WeddingData';
import WeddingDate from "../wedding-information/WeddingDate";

interface Route {
    path: string,
    name: string,
    icon: ReactElement,
    element: () => (JSX.Element)
}

// Placeholder Components
const AboutUs = () => <h1>About Us</h1>;
const Contact = () => <h1>Contact</h1>;
const RSVP = () => <WeddingDate weddingData={weddingData} />;
const Venue = () => <h1>Venue</h1>;
const Accommodations = () => <h1>Accommodations</h1>;
const Registry = () => <h1>Registry</h1>;

const Routes: Route[] = [
    {
        path: '/about',
        name: 'About',
        icon: <People />,
        element: AboutUs
    },
    {
        path: '/contact',
        name: 'Contact',
        icon: <Phone />,
        element: Contact
    },
    {
        path: '/rsvp',
        name: 'RSVP',
        icon: <EditCalendar />,
        element: RSVP
    },
    {
        path: '/venue',
        name: 'Venue',
        icon: <LocationOn />,
        element: Venue
    },
    {
        path: '/accommodations',
        name: 'Accommodations',
        icon: <Info />,
        element: Accommodations
    },
    {
        path: '/registry',
        name: 'Registry',
        icon: <Redeem />,
        element: Registry
    },
]

export default Routes;