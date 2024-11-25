import { useEffect, useMemo, useRef, useState } from 'react';
import { HashRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { default as siteRoutes } from './Routes';
import CssBaseline from '@mui/material/CssBaseline';
import Box from '@mui/material/Box';
import Drawer from '@mui/material/Drawer';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import { alpha, ThemeProvider } from '@mui/material/styles';
import theme from './Theme';
import Header from './components/Header';
import Sidebar from './components/Sidebar';
import { AnimatePresence, motion } from 'framer-motion'
import ChatButton from './components/ChatButton';
import ErrorContainer from './components/ErrorContainer';
import { useTitle } from './hooks/useTitle';
import { useTranslation } from 'react-i18next';
import Background from './components/Background/Background';
import { useIsMobile } from './hooks/useIsMobile';
import ErrorBoundary from './components/ErrorBoundary';

const drawerWidth = 240;

function App() {
  const { t } = useTranslation();

  const { isMobile } = useIsMobile();

  const scrollboxRef = useRef<HTMLElement>();
  const [currentLocation, setCurrentLocation] = useState(window.location.href);
  const handleWindowHistoryChange = () => {
    setSidebarOpen(false);
    setCurrentLocation(window.location.href);
    scrollboxRef.current?.scrollTo({
      'top': 0,
      'behavior': 'instant'
    })
  }
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const handleToggleSidebar = () => setSidebarOpen(!sidebarOpen);

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

  const background = useMemo(() => <Background />, []);

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
              position: 'relative',
              [`& .MuiDrawer-paper`]: {
                width: drawerWidth,
                boxSizing: 'border-box',
                top: (theme) => `${(Number(theme.mixins.toolbar.minHeight!) + 5)}px`,
                background: alpha(theme.palette.background.paper, 0.9)
              },
            }}
          >
            <Sidebar Routes={routes} />
          </Drawer>
          <ErrorBoundary>
            {background}
          </ErrorBoundary>
          <Box ref={scrollboxRef} component="main" sx={{
            top: (theme) => `${(Number(theme.mixins.toolbar.minHeight!) + 5)}px`,
            position: 'relative',
            overflow: 'scroll',
            height:  (theme) => `calc(100vh - ${(Number(theme.mixins.toolbar.minHeight!) + 5)}px)`,
            width: '100%',
            p: {
              xs: 1,
              sm: 3
            },
            pt: {
              xs: 2,
              sm: 3
            },
          }}>
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
                        <ErrorBoundary>
                          {route.element}
                        </ErrorBoundary>
                      </motion.div>
                    } />
                  ))}
                  <Route path="/error" element={<ErrorContainer />} />
                  <Route path="/" element={<DefaultRedirect />} />
                  <Route path="*" element={<Navigate to="/error" replace />} />
                </Routes>
              </AnimatePresence>
          </Box>
        </Box>
      </Router>
      <ErrorBoundary>
        <ChatButton isOpen={chatOpen} isMobile={isMobile} toggle={toggleChat} />
      </ErrorBoundary>
    </ThemeProvider>
  );
}

export default App;