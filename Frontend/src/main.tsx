import React, { Suspense } from 'react'
import ReactDOM from 'react-dom/client'
import './i18n';
import App from './App.tsx'
import './index.css'
import { AnimatePresence, motion } from 'framer-motion';
import LoadingSpinner from './components/LoadingSpinner.tsx';
import { IsMobileProvider } from './hooks/useIsMobile.tsx';
import ErrorBoundary from './components/ErrorBoundary.tsx';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ErrorBoundary>
      <AnimatePresence>
        <Suspense fallback={
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.2 }}
          >
            <LoadingSpinner />
          </motion.div>
        }>
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.5 }}
          >
            <IsMobileProvider>
              <App />
            </IsMobileProvider>
          </motion.div>
        </Suspense>
      </AnimatePresence>
    </ErrorBoundary>
  </React.StrictMode>
)
