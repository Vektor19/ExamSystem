import { createTheme } from "@mui/material/styles";

const theme = createTheme({
  palette: {
    primary: {
      main: "#29223C",
      dark: "#29223C",
      light: "#F4F3F8",
    },
    secondary: {
      main: "#F4F3F8",
      dark: "#29223C",
      light: "#29223C",
    },
  },
  typography: {
    fontFamily:
      '"Montserrat", "Roboto", "Helvetica", "Arial", "Lato", sans-serif',
  },
});

declare module "@mui/material/styles" {
  interface Palette {
    hoverTextPrimaryColor: Palette["primary"];
  }
  interface PaletteOptions {
    hoverTextPrimaryColor?: PaletteOptions["primary"];
  }
}

theme.palette.hoverTextPrimaryColor = theme.palette.augmentColor({
  color: {
    main: "#29223C",
  },
});

export default theme;
