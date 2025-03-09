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
            main: '#84937c'
        },
        secondary: {
            main: '#869eb6'
        },
    }
});

export default theme;