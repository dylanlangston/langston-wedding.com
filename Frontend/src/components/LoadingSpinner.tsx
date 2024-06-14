import CircularProgress from '@mui/material/CircularProgress';
import { Box } from '@mui/material';

const LoadingSpinner = () => {
  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: 'calc(100vh - 40px)',
        width: 'calc(100vw - 40px)',
      }}
    >
      <CircularProgress size="max(50px, min(10vw, 200px))" variant="indeterminate" disableShrink={true} />
    </Box>
  );
};

export default LoadingSpinner;
