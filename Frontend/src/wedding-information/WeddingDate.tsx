import * as React from 'react';
import Typography from '@mui/material/Typography';
import { WeddingEvent } from './WeddingEvent';
import { Card } from '@mui/material';

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
    <Card
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        p: 4,
        margin: 'auto',
        boxShadow: 3,
        borderRadius: 2,
        maxWidth: '600px'
      }}
    >
    <Typography variant="h5">
      Save the Date: {formattedDate}<br/>
      Venue: {weddingData.venue.name}<br/>
      Location: {weddingData.venue.address.getFullAddress()}
    </Typography>
    </Card>
  );
};

export default WeddingDate;