import React from "react";
import { Grid, Box, Typography, Container, Card } from "@mui/material";
import { motion } from "framer-motion";

const AboutUs: React.FC = () => {
  return (
    <Card sx={{ p: {
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
        initial={{ opacity: 0, y: 50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.6 }}
      >
        <Typography variant="h3" sx={{ fontWeight: "bold", mb: 2 }}>
          About Us
        </Typography>
      </motion.div>

      <Grid container spacing={4}>
        {/* Upper Image */}
        <Grid item xs={12}>
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ delay: 0.3, duration: 0.6 }}
          >
            <Box
              sx={{
                width: "100%",
                height: 200,
                backgroundColor: "grey.300",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
              }}
            >
              <Typography variant="body2" color="textSecondary">
                Image Placeholder
              </Typography>
            </Box>
          </motion.div>
        </Grid>

        {/* Paragraph and Smaller Image */}
        <Grid item xs={12} md={6}>
          <motion.div
            initial={{ opacity: 0, x: -50 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ delay: 0.6, duration: 0.6 }}
          >
            <Typography variant="body1" color="textSecondary" paragraph>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
              eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas
              dui id ornare arcu. Dui sapien eget mi proin. Est pellentesque elit
              ullamcorper dignissim cras tincidunt. Orci eu lobortis elementum nibh
              tellus molestie nunc non blandit.
            </Typography>
          </motion.div>
        </Grid>
        <Grid item xs={12} md={6}>
          <motion.div
            initial={{ opacity: 0, x: 50 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ delay: 0.6, duration: 0.6 }}
          >
            <Box
              sx={{
                width: "100%",
                height: 200,
                backgroundColor: "grey.300",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
              }}
            >
              <Typography variant="body2" color="textSecondary">
                Image Placeholder
              </Typography>
            </Box>
          </motion.div>
        </Grid>

        {/* Bottom Section */}
        <Grid item xs={12}>
          <motion.div
            initial={{ opacity: 0, y: 50 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.9, duration: 0.6 }}
          >
            <Typography variant="body1" color="textSecondary" paragraph>
              Nulla posuere sollicitudin aliquam ultrices. Nunc mi ipsum faucibus
              vitae. Dolor sit amet consectetur adipiscing elit. Vestibulum lectus
              mauris ultrices eros in cursus. Quisque non tellus orci ac auctor
              augue. Enim tortor at auctor urna nunc id cursus metus.
            </Typography>
          </motion.div>
        </Grid>

        {/* Bottom Image */}
        <Grid item xs={12}>
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ delay: 1.2, duration: 0.6 }}
          >
            <Box
              sx={{
                width: "100%",
                height: 200,
                backgroundColor: "grey.300",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
              }}
            >
              <Typography variant="body2" color="textSecondary">
                Image Placeholder
              </Typography>
            </Box>
          </motion.div>
        </Grid>
      </Grid>
    </Card>
  );
};

export default AboutUs;
