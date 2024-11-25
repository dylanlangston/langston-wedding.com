import Typography from '@mui/material/Typography';
import { useTranslation } from 'react-i18next';
import Box from '@mui/material/Box'; // Import Box component

type ErrorContainerProps = {
  message?: string
};


const ErrorContainer: React.FC<ErrorContainerProps> = ({ message })=> {
  const { t } = useTranslation();

  if (!message) message = t("Error");

  return (
    <Box sx={{
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      textAlign: 'center',
      height: (theme) => `calc(100vh - calc(${theme.components?.MuiAppBar?.defaultProps?.style?.height} * 2))`,
      userSelect: 'none'
    }}>
      {/* Use Box component for centering */}
      <Typography variant="h2">
        ¯\_(ツ)_/¯
      </Typography>
      <br />
      <Typography variant="h3">
        {message}
      </Typography>
    </Box>
  );
};

export default ErrorContainer;