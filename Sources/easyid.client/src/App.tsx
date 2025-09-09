import "./App.css";
import { Box, CssBaseline } from "@mui/material";
import { LoginPage } from "./pages";
import AppShell from "./pages/AppShell";
import ProtectedRoute from "./routes/ProtectedRoute";
import "react-toastify/dist/ReactToastify.css";
import { ConfirmProvider } from "material-ui-confirm";
import { ThemeProvider } from "./contexts/ThemeContext";
import { BrowserRouter, Route, Routes, Navigate } from "react-router-dom";

function App() {
  return (
    <Box className="app">
      <ThemeProvider>
        <ConfirmProvider>
          <BrowserRouter basename="/">
            <Routes>
              <Route path="/" element={<Navigate to="/app" replace />} />
              <Route path="/login" element={<LoginPage />} />
              <Route
                path="/app"
                element={
                  <ProtectedRoute>
                    <AppShell />
                  </ProtectedRoute>
                }
              />
              <Route path="*" element={<Navigate to="/app" replace />} />
            </Routes>
          </BrowserRouter>
          <CssBaseline enableColorScheme={true} />
        </ConfirmProvider>
      </ThemeProvider>
    </Box>
  );
}

export default App;
