import "./App.css";
import ProfilePage from "./pages/ProfilePage";
import "react-toastify/dist/ReactToastify.css";
import { Box, CssBaseline } from "@mui/material";
import { LoginPage, DashboardPage } from "./pages";
import AppLayout from "./components/layout/AppLayout";
import ProtectedRoute from "./routes/ProtectedRoute";
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
                    <AppLayout
                      title="EasyID"
                      sidebarItems={[
                        {
                          key: "dashboard",
                          label: "Dashboard",
                          route: "/app",
                          order: 1,
                        },
                        {
                          key: "profile",
                          label: "Profile",
                          route: "/app/profile",
                          order: 100,
                        },
                      ]}
                    />
                  </ProtectedRoute>
                }
              >
                <Route index element={<DashboardPage />} />
                <Route path="profile" element={<ProfilePage />} />
              </Route>
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
