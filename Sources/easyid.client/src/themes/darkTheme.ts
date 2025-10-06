import { createTheme } from "@mui/material";

export const darkTheme = createTheme({
  palette: {
    mode: "dark",
    primary: {
      main: "rgb(254, 108, 0)",
    },
    secondary: {
      main: "rgb(254, 108, 0)",
    },
    background: {
      default: "rgba(53, 53, 53, 0.9)",
      paper: "rgba(32, 32, 32, 0.9)",
    },
    text: {
      primary: "rgb(210, 210, 210)",
      secondary: "rgb(200, 200, 200)",
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

export default darkTheme;
