// Import necessary libraries
import React, { createContext, useContext, useState, ReactNode, useEffect } from 'react';
import BrowserDetector from 'browser-dtector';

// Define the type for the context value
interface IsMobileContextType {
  setIsMobile: React.Dispatch<React.SetStateAction<boolean>>;
  isMobile: boolean;
}

// Create the context with an initial value of null
const IsMobileContext = createContext<IsMobileContextType | undefined>(undefined);

// Create a provider component
interface IsMobileProviderProps {
  children: ReactNode;
}

export const IsMobileProvider: React.FC<IsMobileProviderProps> = ({ children }) => {
  // Hook to detect if the device is mobile
  const detector = new BrowserDetector();
  const [isMobile, setIsMobile] = useState(detector.parseUserAgent(window.navigator.userAgent).isMobile);

  const handleWindowSizeChange = () => {
    setIsMobile(detector.parseUserAgent(window.navigator.userAgent).isMobile);
  };

  useEffect(() => {
    window.addEventListener('resize', handleWindowSizeChange);
    return () => {
      window.removeEventListener('resize', handleWindowSizeChange);
    };
  }, []);

  return (
    <IsMobileContext.Provider value={{ isMobile, setIsMobile }}>
      {children}
    </IsMobileContext.Provider>
  );
};

// Create a custom hook for consuming the context
export const useIsMobile = (): IsMobileContextType => {
  const context = useContext(IsMobileContext);
  if (!context) {
    throw new Error('useIsMobile must be used within an IsMobileProvider');
  }
  return context;
};