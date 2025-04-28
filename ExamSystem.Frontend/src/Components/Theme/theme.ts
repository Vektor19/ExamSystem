import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      main: '#29223C',
      dark: '#F4F3F8',
      light: '#29223C',
    },
    secondary: {
      main: '#F4F3F8',
      dark: '#29223C',
        light: '#29223C',

    },
  },
  typography: {
    fontFamily: '"Montserrat", "Roboto", "Helvetica", "Arial", sans-serif',
  },
});

declare module '@mui/material/styles' {
  interface Palette {
    customColor: Palette['primary'];
  }
  interface PaletteOptions {
    customColor?: PaletteOptions['primary'];
  }
}

theme.palette.customColor = theme.palette.augmentColor({
  color: {
    main: '#29223C',
  },
});

export default theme;
