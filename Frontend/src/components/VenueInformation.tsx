import React, { useEffect, useRef, useState } from 'react';
import { Box, Typography, Button, ButtonGroup, Card } from '@mui/material';
import { motion } from 'framer-motion';
import L, { LatLngExpression } from "leaflet";
import "leaflet/dist/leaflet.css";
import GoogleIcon from '@mui/icons-material/Google';
import AppleIcon from '@mui/icons-material/Apple';
import TravelExploreIcon from '@mui/icons-material/TravelExplore';

// Fix missing marker icons issue in Vite/Webpack
import markerIcon from "leaflet/dist/images/marker-icon.png";
import markerShadow from "leaflet/dist/images/marker-shadow.png";

const location: LatLngExpression = [-33.860664, 151.208138];

// Define tile layers
const streetLayer = L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
  attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
});

const satelliteLayer = L.tileLayer(
  "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
  {
    attribution: "&copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community",
  }
);

const Map: React.FC = () => {
  const mapRef = useRef<HTMLDivElement | null>(null);
  const mapInstance = useRef<L.Map | null>(null);
  const tileLayerRef = useRef<L.TileLayer | null>(null);
  const [isSatellite, setIsSatellite] = useState(false);

  useEffect(() => {
    if (!mapRef.current || mapInstance.current) return;

    // Initialize map
    mapInstance.current = L.map(mapRef.current).setView(location, 50);

    // Set the initial tile layer
    streetLayer.addTo(mapInstance.current);
    tileLayerRef.current = streetLayer;

    // Fix marker icon issues
    const defaultIcon = L.icon({
      iconUrl: markerIcon,
      shadowUrl: markerShadow,
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
    });

    // Add a marker
    L.marker(location, { icon: defaultIcon })
      .addTo(mapInstance.current)
      .bindPopup("Venue Location")
      .openPopup();

    return () => {
      mapInstance.current?.remove();
      mapInstance.current = null;
    };
  }, []);

  // Toggle map layer
  const toggleLayer = () => {
    if (!mapInstance.current || !tileLayerRef.current) return;
    setIsSatellite((prev) => !prev);

    tileLayerRef.current?.remove();
    const newTileLayer = (isSatellite ? streetLayer : satelliteLayer).addTo(mapInstance.current);

    tileLayerRef.current = newTileLayer;
  };

  return (
    <div style={{ position: "relative", width: "100%", height: "80vh" }}>
      <div
        onClick={toggleLayer}
        style={{
          position: "absolute",
          top: "10px",
          right: "10px",
          zIndex: 1000,
          backgroundColor: "white",
          padding: "10px",
          borderRadius: "50%",
          boxShadow: "0px 2px 5px rgba(0,0,0,0.3)",
          cursor: "pointer",
          userSelect: "none"
        }}
        title="Toggle Satellite View"
      >
        {isSatellite ? "🗺️" : "🛰️"}
      </div>
      <div ref={mapRef} style={{ height: "100%", width: "100%" }} />
    </div>
  );
};

const VenueInformation: React.FC = () => {
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
        style={{display: 'flex', flexDirection: 'column', width: '100%'}}
      >
        <Box sx={{ pt: 2 }}>
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
            <Button startIcon={<GoogleIcon />}>Google Maps</Button>
            <Button startIcon={<AppleIcon />}>Apple Maps</Button>
            <Button startIcon={<TravelExploreIcon />}>Open Street Maps</Button>
          </ButtonGroup>
        </Box>
        <Box sx={{ mt: 2, width: '100%' }}>
          <Map />
        </Box>
      </motion.div>
    </Card>
  );
};

export default VenueInformation;