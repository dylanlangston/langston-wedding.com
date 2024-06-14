import React, { Suspense } from 'react'
import ReactDOM from 'react-dom/client'
import './i18n';
import App from './App.tsx'
import './index.css'
import { AnimatePresence, motion } from 'framer-motion';
import LoadingSpinner from './components/LoadingSpinner.tsx';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
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
          <App />
        </motion.div>
      </Suspense>
    </AnimatePresence>
  </React.StrictMode>
)
