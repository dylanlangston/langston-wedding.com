import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

const ErrorPage = () => {
  return (
    <Box 
      display="flex" 
      justifyContent="center" 
      alignItems="center" 
      textAlign="center"
    >
      <Typography variant="h1">
        ¯\_(ツ)_/¯<br />An error has occurred, sorry!
      </Typography>
    </Box>
  );
};

export default ErrorPage;
