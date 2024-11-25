import { Tooltip, Zoom, Box, Typography } from '@mui/material';
import ChatIcon from '@mui/icons-material/Chat';
import { useTranslation } from 'react-i18next';
import ChatWindow from './ChatWindow';
import { motion, AnimatePresence } from 'framer-motion';

interface ChatButtonProps {
  isOpen: boolean,
  isMobile: boolean,
  toggle: () => void;
}

const ChatButton: React.FC<ChatButtonProps> = ({ isOpen, isMobile, toggle }) => {
  const { t } = useTranslation();

  return (
    <AnimatePresence>
      {isOpen ? (
          <ChatWindow isMobile={isMobile} toggle={() => toggle()} />
      ) : (
        <motion.div
          key="chatbutton"
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          exit={{ opacity: 0 }}
          transition={{ duration: 0.5 }}
        >
          {/* <Tooltip key="chatButton" title={t("Chat Tooltip")} placement="left" TransitionComponent={Zoom}> */}
            <Box
              sx={{
                position: 'fixed',
                bottom: 20,
                right: 20,
                backgroundColor: 'primary.main',
                color: 'common.white',
                display: 'flex',
                alignItems: 'center',
                padding: '8px 16px',
                borderRadius: '20px',
                '&:hover': {
                  backgroundColor: 'primary.dark',
                },
                userSelect: 'none',
                cursor: 'pointer'
              }}
              onClick={toggle}
            >
              <Typography variant="body2">{t('Chat')}</Typography>
              <ChatIcon sx={{ ml: 1 }} />
            </Box>
          {/* </Tooltip> */}
        </motion.div>
      )}
    </AnimatePresence>
  );
};

export default ChatButton;