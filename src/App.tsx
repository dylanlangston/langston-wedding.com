import { useEffect, useState } from 'react';
import { HashRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { default as siteRoutes } from './Routes';
import CssBaseline from '@mui/material/CssBaseline';
import Box from '@mui/material/Box';
import Drawer from '@mui/material/Drawer';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import { ThemeProvider } from '@mui/material/styles';
import theme from './Theme';
import Header from './components/Header';
import Sidebar from './components/Sidebar';
import { AnimatePresence, motion } from 'framer-motion'
import BrowserDetector from 'browser-dtector';
import ChatButton from './components/ChatButton';

const drawerWidth = 240;

const detector = new BrowserDetector();

function App() {
  const [canToggle, setCanToggle] = useState(detector.parseUserAgent(window.navigator.userAgent).isMobile);
  const handleWindowSizeChange = () => setCanToggle(detector.parseUserAgent(window.navigator.userAgent).isMobile);
  const [currentLocation, setCurrentLocation] = useState(window.location.href);
  const handleWindowHistoryChange = () => {
    setSidebarOpen(false);
    setCurrentLocation(window.location.href);
  }
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const handleToggleSidebar = () => setSidebarOpen(!sidebarOpen);

  useEffect(() => {
    window.addEventListener('resize', handleWindowSizeChange);
    return () => {
      window.removeEventListener('resize', handleWindowSizeChange);
    }
  }, []);

  useEffect(() => {
    window.addEventListener('popstate', handleWindowHistoryChange);
    return () => {
      window.removeEventListener('popstate', handleWindowHistoryChange);
    }
  }, []);

  const toggleChat = () => {};

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Box sx={{ display: 'flex' }}>
          <AppBar
            position="fixed"
            sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
          >
            <Header canToggle={canToggle} toggleSidebar={handleToggleSidebar} />
          </AppBar>
          <Drawer
            variant={canToggle ? "temporary" : "permanent"}
            open={sidebarOpen}
            onClose={handleToggleSidebar}
            sx={{
              width: drawerWidth,
              flexShrink: 0,
              [`& .MuiDrawer-paper`]: {
                width: drawerWidth,
                boxSizing: 'border-box',
              },
            }}
          >
            <Sidebar />
          </Drawer>
          <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
            <Toolbar />
            <AnimatePresence mode="wait">
              <Routes>
                <Route path="/" element={<Navigate to="about" replace />} />
                {siteRoutes.map((route) => (
                  <Route path={route.path} element={
                    <motion.div
                      key={currentLocation}
                      initial={{ opacity: 0 }}
                      animate={{ opacity: 1 }}
                      exit={{ opacity: 0 }}
                      transition={{ duration: 0.2 }}
                    >
                      {route.element}
                    </motion.div>
                  } />
                ))}
              </Routes>
            </AnimatePresence>
          </Box>
        </Box>
      </Router>
      <ChatButton toggle={toggleChat} />
    </ThemeProvider>
  );
}

export default App;