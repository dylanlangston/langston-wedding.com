import { Tooltip, Zoom, Box, Typography } from '@mui/material';
import ChatIcon from '@mui/icons-material/Chat';

interface ChatButtonProps {
  toggle: () => void;
}

const ChatButton: React.FC<ChatButtonProps> = ({toggle}) => {
  return (
    <Tooltip title="Open Chat" placement="left" TransitionComponent={Zoom}>
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
        <Typography variant="body2">Chat</Typography> 
        <ChatIcon sx={{ ml: 1 }} /> {/* Add some spacing between icon and text */}
      </Box>
    </Tooltip>
  );
};

export default ChatButton;