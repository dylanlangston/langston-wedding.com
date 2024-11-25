import { alpha, createTheme } from '@mui/material/styles';

const theme = createTheme({
    components: {
        MuiAppBar: {
            styleOverrides: {
              root: ({ theme }) => ({
                height: "60px",
                backgroundColor: alpha(theme.palette.primary.main, 0.5)
              })
          }
        },
        MuiCard: {
            styleOverrides: {
                root: ({theme}) => ({
                    background: alpha(theme.palette.background.paper, 0.9),
                    borderRadius: '10px'
                })
            }
        }
    },
    palette: {
        primary: {
            main: '#d94343'
        },
        secondary: {
            main: '#d9438e'
        },
    }
});

export default theme;