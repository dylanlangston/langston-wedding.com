import * as React from 'react';
import IconButton from '@mui/material/IconButton';
import "./Hamburger.css";

interface HamburgerProps {
    sidebarOpen: boolean,
    toggleSidebar: () => void;
}

const Hamburger: React.FC<HamburgerProps> = ({ sidebarOpen, toggleSidebar }) => {
    return (<IconButton
        size="large"
        edge="start"
        aria-label="open drawer"
        onClick={toggleSidebar}
        className={'hamburger hamburger--arrow' + (sidebarOpen ? ' is-active' : '')}
    >
        <span className="hamburger-box">
            <span className="hamburger-inner"></span>
        </span>
    </IconButton>
    );
};

export default Hamburger;