import React, { useState } from 'react';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import { useTranslation } from 'react-i18next';
import { motion, AnimatePresence } from 'framer-motion';

interface Message {
    content: string;
    sender: 'user' | 'bot';
}

interface ChatWindowProps {
    isMobile: boolean,
    toggle: () => void;
}

const ChatWindow: React.FC<ChatWindowProps> = ({ isMobile, toggle }) => {
    const { t } = useTranslation();

    const [messages, setMessages] = useState<Message[]>([]);
    const [newMessage, setNewMessage] = useState('');

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setNewMessage(event.target.value);
    };

    const handleSendMessage = () => {
        if (newMessage.trim() !== '') {
            setMessages([...messages, { content: newMessage, sender: 'user' }]);
            setNewMessage('');
        }
    };

    return (
        <Box
            sx={{
                position: 'fixed',
                bottom: 10,
                right: 10,
                zIndex: (theme) => theme.zIndex.drawer + 10
            }}
        >
            <motion.div
                key="chatwindow"
                initial={{ x: "100vw", opacity: isMobile ? 0 : 1 }}
                animate={{ x: "0vw", opacity: isMobile ? 1 : 1 }}
                exit={{ x: "100vw", opacity: isMobile ? 0 : 1 }}
                transition={{ type: "spring", stiffness: 200, damping: 20 }}
            >
                <Box
                    sx={{
                        width: isMobile ? 'calc(100vw - 20px)' : 400,
                        height: isMobile ? 'calc(100vh - 20px)' : 500,
                        boxShadow: isMobile ? '-100vmax -100vmax 10px 200vmax rgba(0, 0, 0, 0.5)' : 24,
                        bgcolor: 'background.paper',
                        borderRadius: 2,
                        display: 'flex',
                        flexDirection: 'column',
                        zIndex: (theme) => theme.zIndex.drawer + 10
                    }}
                >
                    <Box
                        sx={{
                            display: 'flex',
                            justifyContent: 'space-between',
                            alignItems: 'center',
                            padding: 1,
                            bgcolor: 'primary.main',
                            borderRadius: 2,
                            color: 'primary.contrastText'
                        }}
                    >
                        <Typography variant="h6" sx={{ paddingLeft: 2, userSelect: 'none' }}>
                            {t("Chat Window")}
                        </Typography>
                        <IconButton
                            aria-label="close"
                            size='large'
                            onClick={toggle}
                            sx={{ color: 'primary.contrastText' }}
                        >
                            <CloseIcon />
                        </IconButton>
                    </Box>


                    <Box sx={{ flexGrow: 1, overflowY: 'auto', padding: 2 }}>
                        {messages.map((message, index) => (
                            <Typography key={index} variant="body1" sx={{ padding: 1 }}>
                                <b>{message.sender}: </b>{message.content}
                            </Typography>
                        ))}
                    </Box>

                    <Box sx={{ display: 'flex', alignItems: 'center', padding: 2 }}>
                        <TextField
                            label={t("Type your message")}
                            value={newMessage}
                            onChange={handleInputChange}
                            fullWidth
                        />
                        <Button variant="contained" onClick={handleSendMessage} sx={{ marginLeft: 2 }}>
                            {t("Send")}
                        </Button>
                    </Box>
                </Box>
            </motion.div>
        </Box>
    );
};

export default ChatWindow;