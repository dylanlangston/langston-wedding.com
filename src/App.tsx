import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { default as siteRoutes } from './shared/Routes';
import CssBaseline from '@mui/material/CssBaseline';
import Box from '@mui/material/Box';
import Drawer from '@mui/material/Drawer';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import { ThemeProvider } from '@mui/material/styles';
import theme from './Theme';
import Header from './shared/Header';
import Sidebar from './shared/Sidebar';
import { AnimatePresence, motion } from 'framer-motion'
import BrowserDetector from 'browser-dtector';

const drawerWidth = 240;

const detector = new BrowserDetector();

function App() {
  const [canToggle, setCanToggle] = useState(detector.parseUserAgent(window.navigator.userAgent).isMobile);
  const handleWindowSizeChange = () => setCanToggle(detector.parseUserAgent(window.navigator.userAgent).isMobile);
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const handleToggleSidebar = () => setSidebarOpen(!sidebarOpen);

  useEffect(() => {
    window.addEventListener('resize', handleWindowSizeChange);
    return () => {
        window.removeEventListener('resize', handleWindowSizeChange);
    }
  }, []);
  

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
              <motion.div
                key={location.pathname}
                initial={{ opacity: 0, x: 0 }}
                animate={{ opacity: 1, x: 0 }}
                exit={{ opacity: 0, x: 0 }}
                transition={{ duration: 0.5 }}
              >
                <Routes>
                  <Route path="/" element={<Navigate to="/about" replace />} />
                  {siteRoutes.map((route) => (
                    <Route path={route.path} element={route.element()} />
                  ))}
                </Routes>
              </motion.div>
            </AnimatePresence>
          </Box>
        </Box>
      </Router>
    </ThemeProvider>
  );
}

export default App;