import React, { useState } from 'react';
import { Box, Typography, Button, ButtonGroup, Card } from '@mui/material';
import { motion } from 'framer-motion';
import { APIProvider, Map } from '@vis.gl/react-google-maps';

const VenueInformation: React.FC = () => {
  const [mapType, setMapType] = useState('roadmap');

  return (
    <Card sx={{
      display: 'flex', p: {
        xs: 2,
        sm: 4
      },
      m: {
        xs: 0,
        sm: 'auto'
      },
      maxWidth: {
        md: '70vw'
      }
    }}>
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <Box sx={{ p: 2 }}>
          <Typography variant="h3" sx={{ fontWeight: 'bold' }} gutterBottom>
            Venue Information
          </Typography>
          <Typography variant="body1" paragraph>
            Augue interdum velit euismod in pellentesque. Aliquet risus feugiat in ante
            metus dictum. Sollicitudin aliquam ultrices sagittis orci a scelerisque.
            Erat velit scelerisque in dictum non. Sed elementum tempus egestas
            sed. Gravida rutrum quisque non tellus. Elementum curabitur vitae nunc
            sed.
          </Typography>
          <ButtonGroup variant="outlined" sx={{ mt: 2 }}>
            <Button onClick={() => setMapType('roadmap')}>Map</Button>
            <Button onClick={() => setMapType('hybrid')}>Satellite</Button>
          </ButtonGroup>

          <Box sx={{ mt: 2, width: '100%', }}>
            <APIProvider apiKey={'AIzaSyCY4c79PtRnkwJGbZh7bmyvJxM2UC1j-4k'}>
              <Map
                style={{ width: '800px', height: '400px' }}
                defaultZoom={13}
      defaultCenter={ { lat: -33.860664, lng: 151.208138 } }
                gestureHandling={'greedy'}
                disableDefaultUI={true}
                minZoom={13}
                maxZoom={13}
                mapTypeId={mapType}
              />
            </APIProvider>
          </Box>
        </Box>
      </motion.div>
    </Card>
  );
};

export default VenueInformation;