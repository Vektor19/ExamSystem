import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./Styles/index.css";
import App from "./Components/App.tsx";
import { HashRouter } from "react-router-dom";
import { ThemeProvider } from "@mui/material/styles";
import theme from "./Components/Theme/theme.ts";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <HashRouter>
      <ThemeProvider theme={theme}>
        <App />
      </ThemeProvider>
    </HashRouter>
  </StrictMode>
);
