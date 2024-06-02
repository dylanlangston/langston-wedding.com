import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { Favorite } from '@mui/icons-material';

const Header: React.FC = () => {
  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1, ml: 2 }}> 
          Dylan & Mia's Wedding
        </Typography>
        <Favorite color="error" />
      </Toolbar>
    </AppBar>
  );
};

export default Header;