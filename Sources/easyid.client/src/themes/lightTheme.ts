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
        "input:-webkit-autofill, textarea:-webkit-autofill, select:-webkit-autofill":
          {
            WebkitTextFillColor: "#e6e6e6",
            WebkitBoxShadow: "0 0 0px 1000px #151A21 inset",
            transition:
              "background-color 999999s ease 0s, color 999999s ease 0s",
            caretColor: "#e6e6e6",
          },
        "input:-webkit-autofill:focus, textarea:-webkit-autofill:focus, select:-webkit-autofill:focus":
          {
            WebkitBoxShadow: "0 0 0px 1000px #151A21 inset",
          },
      },
    },
  },
});

export default lightTheme;
