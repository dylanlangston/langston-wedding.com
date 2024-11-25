import { ReactElement } from "react";
import { EditCalendar, People, Phone, LocationOn, Info, Redeem } from '@mui/icons-material';
import { weddingData } from './wedding-information/WeddingData';
import WeddingDate from "./wedding-information/WeddingDate";
import { useTranslation } from "react-i18next";
import GetInContact from "./components/GetInContact";
import AboutUs from "./components/AboutUs";
import VenueInformation from "./components/VenueInformation";
import Accommodations from "./components/Accommodations";
import Registries from "./components/Registries";

export interface Route {
    path: string,
    name: string,
    icon: ReactElement,
    element: ReactElement
}

const Routes: () => Route[] = () => {
    const { t } = useTranslation();

    // Placeholder Components
    const About = () => <AboutUs/>
    const Contact = () => <GetInContact/>; 
    const RSVP = () => <WeddingDate weddingData={weddingData} />;
    const Venue = () => <VenueInformation/>;
    const Hotels = () => <Accommodations/>;
    const Registry = () => <Registries/>;

    return [
        {
            path: 'about',
            name: t('About Us'),
            icon: <People />,
            element: About()
        },
        {
            path: 'contact',
            name: t('Contact'),
            icon: <Phone />,
            element: Contact()
        },
        {
            path: 'rsvp',
            name: t('RSVP'),
            icon: <EditCalendar />,
            element: RSVP()
        },
        {
            path: 'venue',
            name: t('Venue'),
            icon: <LocationOn />,
            element: Venue()
        },
        {
            path: 'accommodations',
            name: t('Accommodations'),
            icon: <Info />,
            element: Hotels()
        },
        {
            path: 'registry',
            name: t('Registry'),
            icon: <Redeem />,
            element: Registry()
        },
    ];
};

export default Routes;