import React, { useState } from 'react';
import { IconButton, Tooltip, Zoom, Box, Typography } from '@mui/material';
import ChatIcon from '@mui/icons-material/Chat';

const ChatButton = () => {
  const [isOpen, setIsOpen] = useState(false);

  const handleClick = () => {
    setIsOpen(!isOpen);
  };

  return (
    <Tooltip title="Open Chat" placement="left" TransitionComponent={Zoom}>
      <Box // Use a Box for flexible styling and layout 
        sx={{
          position: 'fixed',
          bottom: 20,
          right: 20,
          zIndex: (theme) => theme.zIndex.drawer + 2,
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
      >
        <Typography variant="body2">Chat</Typography> 
        <ChatIcon sx={{ ml: 1 }} /> {/* Add some spacing between icon and text */}
      </Box>
    </Tooltip>
  );
};

export default ChatButton;