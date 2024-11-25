import React, { useState } from 'react';
import { Card, CardContent, CardMedia, Typography, Button, CardActions, CardActionArea } from '@mui/material';
import { motion } from 'framer-motion';
import PhotoIcon from '@mui/icons-material/Photo';

interface HotelCardProps {
    title: string;
    body: string;
    image: string;
}

const HotelCard: React.FC<HotelCardProps> = ({ title, body, image }) => {
    const [imageLoaded, setImageLoaded] = useState(false);

    return (
        <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5 }}
        >
            <Card sx={{
                display: 'flex',
                maxWidth: {
                    md: '70vw'
                },
                mb: 2,
                mx: 'auto'
            }}>
                <CardActionArea sx={{
                    display: 'flex',
                    flexDirection: {
                        xs: 'column',
                        md: 'row'
                    },
                }}>

                    <CardMedia component='img' sx={{ width: { md: '40%' }, height: '100%', backgroundColor: 'grey', visibility: imageLoaded ? 'visible' : 'collapse' }} image={image} onLoad={() => setImageLoaded(true)} />
                    {!imageLoaded && <CardMedia sx={{ width: { md: '40%' }, height: '100%', backgroundColor: 'grey', }} onLoad={() => setImageLoaded(true)} >
                        <PhotoIcon sx={{ fontSize: 60, color: 'white' }} />
                    </CardMedia>}
                    <CardContent sx={{ width: { md: '60%' }, height: '100%', flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
                        <Typography variant="h5" component="div">
                            {title}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            {body}
                        </Typography>
                        <CardActions sx={{ alignSelf: 'end', mt: 'auto' }}>
                            <Button variant="contained">WEBSITE</Button>
                        </CardActions>
                    </CardContent>
                </CardActionArea>
            </Card>
        </motion.div>
    );
};

export default HotelCard;