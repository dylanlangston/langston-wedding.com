import * as React from 'react';
import Toolbar from '@mui/material/Toolbar';
import Box from '@mui/material/Box';
import List from '@mui/material/List';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import Routes from './Routes';
import { ListItemButton } from '@mui/material';

const Sidebar: React.FC = () => {
    return (
        <>
            <Toolbar />
            <Box sx={{ overflow: 'auto' }}>
                <List>
                    {Routes.map((route) => (
                        <ListItemButton key={route.name} component="a" href={route.path}>
                            <ListItemIcon>{route.icon}</ListItemIcon>
                            <ListItemText primary={route.name} />
                        </ListItemButton>
                    ))}
                </List>
            </Box>
        </>
    );
};

export default Sidebar;