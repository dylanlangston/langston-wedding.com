import * as React from 'react';
import Typography from '@mui/material/Typography';
import { WeddingEvent } from './WeddingEvent';

interface WeddingDateProps {
  weddingData: WeddingEvent; 
}

const WeddingDate: React.FC<WeddingDateProps> = ({ weddingData }) => {
  const formattedDate = new Date(weddingData.date).toLocaleDateString('en-US', { 
    weekday: 'long',
    year: 'numeric', 
    month: 'long', 
    day: 'numeric' 
  });

  return (
    <Typography variant="h5">
      Save the Date: {formattedDate}<br/>
      Venue: {weddingData.venue.name}<br/>
      Location: {weddingData.venue.address.getFullAddress()}
    </Typography>
  );
};

export default WeddingDate;