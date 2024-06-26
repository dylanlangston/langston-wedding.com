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
import ErrorPage from './ErrorPage';
import { useTitle } from './hooks/useTitle';
import { useTranslation } from 'react-i18next';

const drawerWidth = 240;

const detector = new BrowserDetector();

function App() {
  const { t } = useTranslation();

  const [isMobile, setIsMobile] = useState(detector.parseUserAgent(window.navigator.userAgent).isMobile);
  const handleWindowSizeChange = () => setIsMobile(detector.parseUserAgent(window.navigator.userAgent).isMobile);
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

  const [chatOpen, setChatOpen] = useState(false);
  const toggleChat = () => setChatOpen(!chatOpen);

  const DefaultRedirect = () => {
    if (window.location.pathname === '/') {
      return <Navigate to="/about" replace />;
    }
    return <Navigate to="/error" replace />;
  };

  const routes = siteRoutes();

  useTitle(t("Header"));

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Box sx={{ display: 'flex' }}>
          <AppBar
            position="fixed"
            sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
          >
            <Header sidebarOpen={sidebarOpen} canToggle={isMobile} toggleSidebar={handleToggleSidebar} />
          </AppBar>
          <Drawer
            variant={isMobile ? "temporary" : "permanent"}
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
            <Sidebar Routes={routes} />
          </Drawer>
          <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
            <Toolbar />
            <AnimatePresence mode="wait">
              <Routes>
                {routes.map((route) => (
                  <Route key={route.path} path={route.path} element={
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
                <Route path="/error" element={<ErrorPage />} />
                <Route path="/" element={<DefaultRedirect />} />
                <Route path="*" element={<Navigate to="/error" replace />} />
              </Routes>
            </AnimatePresence>
          </Box>
        </Box>
      </Router>
      <ChatButton isOpen={chatOpen} isMobile={isMobile} toggle={toggleChat} />
    </ThemeProvider>
  );
}

export default App;