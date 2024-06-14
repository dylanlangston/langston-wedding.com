import { ReactElement } from "react";
import { EditCalendar, People, Phone, LocationOn, Info, Redeem } from '@mui/icons-material';
import { weddingData } from './wedding-information/WeddingData';
import WeddingDate from "./wedding-information/WeddingDate";
import { useTranslation } from "react-i18next";

export interface Route {
    path: string,
    name: string,
    icon: ReactElement,
    element: ReactElement
}

const Routes: () => Route[] = () => {
    const { t } = useTranslation();

    // Placeholder Components
    const AboutUs = () => <h1>{t('About Us')}</h1>;
    const Contact = () => <h1>{t('Contact')}</h1>;
    const RSVP = () => <WeddingDate weddingData={weddingData} />;
    const Venue = () => <h1>{t('Venue')}</h1>;
    const Accommodations = () => <h1>{t('Accommodations')}</h1>;
    const Registry = () => <h1>{t('Registry')}</h1>;

    return [
        {
            path: 'about',
            name: t('About Us'),
            icon: <People />,
            element: AboutUs()
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
            element: Accommodations()
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