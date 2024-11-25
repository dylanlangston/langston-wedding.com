import * as React from 'react';
import Toolbar from '@mui/material/Toolbar';
import Box from '@mui/material/Box';
import List from '@mui/material/List';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import { Route } from '../Routes';
import { Link, ListItemButton, Typography } from '@mui/material';
import { useLocation } from 'react-router-dom';

interface SidebarProps {
    Routes: Route[]
}

const Sidebar: React.FC<SidebarProps> = ({ Routes }) => {
    const location = useLocation();

    return (
        <>
            <Box sx={{ overflow: 'auto', display: 'flex', flexDirection: 'column', height: (theme) => `calc(100vh - ${(Number(theme.mixins.toolbar.minHeight!) + 15)}px)`, }}>
                <List>
                    {Routes.map((route) => (
                        <ListItemButton selected={location.pathname === `/${route.path}`} key={route.name} component="a" href={window.location.protocol + "//" + window.location.host + "/#/" + route.path}>
                            <ListItemIcon>{route.icon}</ListItemIcon>
                            <ListItemText primary={route.name} />
                        </ListItemButton>
                    ))}
                </List>
                <Typography sx={{ mt: 'auto', mx: 'auto' }} variant='caption'>©{new Date().getFullYear()} - <Link href="https://dylanlangston.com" target="_blank" rel="noopener noreferrer">Dylan Langston</Link></Typography>
            </Box>
        </>
    );
};

export default Sidebar;