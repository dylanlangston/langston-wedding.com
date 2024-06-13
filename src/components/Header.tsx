import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import { Favorite } from '@mui/icons-material';

interface HeaderProps {
  canToggle: boolean,
  toggleSidebar: () => void;
}

const Header: React.FC<HeaderProps> = ({ canToggle, toggleSidebar }) => {
  return (
    <AppBar position="static">
      <Toolbar>
        {canToggle ? <IconButton
          size="large"
          edge="start"
          color="inherit"
          aria-label="open drawer"
          sx={{ mr: 2 }}
          onClick={toggleSidebar}
        >
          <MenuIcon />
        </IconButton> : null}

        <Typography variant="h6" component="div" sx={{ flexGrow: 1, ml: 2 }}>
          Dylan & Mia's Wedding
        </Typography>
        <Favorite color="error" />
      </Toolbar>
    </AppBar>
  );
};

export default Header;