import { createTheme } from "@mui/material";

export const lightTheme = createTheme({
  palette: {
    mode: "light",
    primary: {
      main: "rgb(254, 108, 0)",
    },
    secondary: {
      main: "rgb(254, 108, 0)",
    },
    background: {
      default: "rgba(255, 255, 255, 0.95)",
      paper: "rgba(255, 255, 255, 0.95)",
    },
    text: {
      primary: "rgb(48, 48, 48)",
      secondary: "rgb(64, 64, 64)",
    },
  },
  typography: {
    fontFamily: '"Ubuntu", serif',
  },
  components: {
    MuiCssBaseline: {
      styleOverrides: {
        "input:-webkit-autofill, textarea:-webkit-autofill, select:-webkit-autofill": {
          WebkitTextFillColor: "rgb(48,48,48)",
          WebkitBoxShadow: "0 0 0px 1000px rgba(255,255,255,0.95) inset !important",
          backgroundColor: "rgba(255,255,255,0.95) !important",
          backgroundClip: "content-box !important",
          transition: "background-color 9999s ease 0s, color 9999s ease 0s",
          caretColor: "rgb(48,48,48)",
          borderRadius: 4,
        },
        "input:-webkit-autofill:focus, textarea:-webkit-autofill:focus, select:-webkit-autofill:focus": {
          WebkitBoxShadow: "0 0 0px 1000px rgba(255,255,255,0.95) inset !important",
          backgroundColor: "rgba(255,255,255,0.95) !important",
        },
        "input:autofill, textarea:autofill, select:autofill": {
          boxShadow: "0 0 0px 1000px rgba(255,255,255,0.95) inset",
          backgroundColor: "rgba(255,255,255,0.95) !important",
        },
      },
    },
  },
});

export default lightTheme;
