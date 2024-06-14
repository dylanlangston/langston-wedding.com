import { Tooltip, Zoom, Box, Typography } from '@mui/material';
import ChatIcon from '@mui/icons-material/Chat';
import { useTranslation } from 'react-i18next';

interface ChatButtonProps {
  toggle: () => void;
}

const ChatButton: React.FC<ChatButtonProps> = ({toggle}) => {
  const { t } = useTranslation();

  return (
    <Tooltip title={t("Chat Tooltip")} placement="left" TransitionComponent={Zoom}>
      <Box
        sx={{
          position: 'fixed',
          bottom: 20,
          right: 20,
          backgroundColor: 'primary.main',
          color: 'common.white',
          display: 'flex', // Align icon and text horizontally
          alignItems: 'center', // Center content vertically
          padding: '8px 16px', // Add some padding around the button content
          borderRadius: '20px', // Round the corners for a softer look
          '&:hover': {
            backgroundColor: 'primary.dark',
          },
        }}
        onClick={toggle}
      >
        <Typography variant="body2">{t('Chat')}</Typography> 
        <ChatIcon sx={{ ml: 1 }} /> {/* Add some spacing between icon and text */}
      </Box>
    </Tooltip>
  );
};

export default ChatButton;