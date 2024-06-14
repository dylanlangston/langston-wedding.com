import { createTheme } from '@mui/material/styles';

const theme = createTheme({
    components: {
        MuiAppBar: {
            defaultProps: {
                style: {
                    height: "60px"
                }
            }
        }
    }
});

export default theme;