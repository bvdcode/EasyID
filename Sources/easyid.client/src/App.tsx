import "./App.css";
import ProfilePage from "./pages/ProfilePage";
import NotFoundPage from "./pages/NotFoundPage";
import "react-toastify/dist/ReactToastify.css";
import { Box, CssBaseline } from "@mui/material";
import {
  LoginPage,
  DashboardPage,
  AppsPage,
  GroupsPage,
  RolesPage,
  PermissionsPage,
  UsersPage,
} from "./pages";
import AppLayout from "./components/layout/AppLayout";
import ProtectedRoute from "./routes/ProtectedRoute";
import { ConfirmProvider } from "material-ui-confirm";
import { ThemeProvider } from "./contexts/ThemeContext";
import { BrowserRouter, Route, Routes, Navigate } from "react-router-dom";
import { PERMISSIONS } from "./constants/app";

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
                          key: "apps",
                          label: "Apps",
                          route: "/app/apps",
                          order: 20,
                          requiredPermissionPrefix: PERMISSIONS.APPS,
                        },
                        {
                          key: "groups",
                          label: "Groups",
                          route: "/app/groups",
                          order: 30,
                          requiredPermissionPrefix: PERMISSIONS.GROUPS,
                        },
                        {
                          key: "roles",
                          label: "Roles",
                          route: "/app/roles",
                          order: 40,
                          requiredPermissionPrefix: PERMISSIONS.ROLES,
                        },
                        {
                          key: "permissions",
                          label: "Permissions",
                          route: "/app/permissions",
                          order: 50,
                          requiredPermissionPrefix: PERMISSIONS.PERMISSIONS,
                        },
                        {
                          key: "users",
                          label: "Users",
                          route: "/app/users",
                          order: 60,
                          requiredPermissionPrefix: PERMISSIONS.USERS,
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
                <Route path="apps" element={<AppsPage />} />
                <Route path="groups" element={<GroupsPage />} />
                <Route path="roles" element={<RolesPage />} />
                <Route path="permissions" element={<PermissionsPage />} />
                <Route path="users" element={<UsersPage />} />
                <Route path="profile" element={<ProfilePage />} />
              </Route>
              <Route path="*" element={<NotFoundPage />} />
            </Routes>
          </BrowserRouter>
          <CssBaseline enableColorScheme={true} />
        </ConfirmProvider>
      </ThemeProvider>
    </Box>
  );
}

export default App;
