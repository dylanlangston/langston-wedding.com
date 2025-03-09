import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { Favorite } from '@mui/icons-material';
import { useTranslation } from 'react-i18next';
import Hamburger from './Hamburger';

interface HeaderProps {
  sidebarOpen: boolean,
  canToggle: boolean,
  toggleSidebar: () => void;
}

const Header: React.FC<HeaderProps> = ({ sidebarOpen, canToggle, toggleSidebar }) => {
  const { t } = useTranslation();

  return (
    <AppBar position="static">
      <Toolbar>
        {canToggle ? <Hamburger sidebarOpen={sidebarOpen} toggleSidebar={() => toggleSidebar()} /> : null}

        <Typography variant="h6" component="div" sx={{ ml: 1, mr: 1, userSelect: 'none' }}>
          {t('Header')}
        </Typography>
        <Favorite color="secondary" />
      </Toolbar>
    </AppBar>
  );
};

export default Header;