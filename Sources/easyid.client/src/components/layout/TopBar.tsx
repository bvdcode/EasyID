import {
  AppBar,
  Box,
  IconButton,
  Toolbar,
  Typography,
  Tooltip,
} from "@mui/material";
import React from "react";
import { authStore } from "../../stores/authStore";
import { userStore } from "../../stores/userStore";
import LogoutIcon from "@mui/icons-material/Logout";
import DarkModeIcon from "@mui/icons-material/DarkMode";
import LightModeIcon from "@mui/icons-material/LightMode";
import { useAppTheme } from "../../contexts/ThemeContext";

interface TopBarProps {
  title?: string;
  onLogout?: () => void;
}

const TopBar: React.FC<TopBarProps> = ({ title = "App", onLogout }) => {
  const user = userStore((s) => s.user);

  const handleLogout = () => {
    authStore.getState().logout();
    userStore.getState().clear();
    if (onLogout) onLogout();
  };

  const { isDarkMode, toggleTheme } = useAppTheme();

  return (
    <AppBar position="static" color="primary" elevation={1}>
      <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
        <Typography variant="h6" component="div">
          {title}
        </Typography>
        <Box display="flex" alignItems="center" gap={2}>
          {user && (
            <Typography variant="body2" sx={{ opacity: 0.85 }}>
              {user.username} ({user.email})
            </Typography>
          )}
          <Tooltip
            title={
              isDarkMode ? "Switch to light theme" : "Switch to dark theme"
            }
          >
            <IconButton color="inherit" onClick={toggleTheme} size="small">
              {isDarkMode ? (
                <LightModeIcon fontSize="small" />
              ) : (
                <DarkModeIcon fontSize="small" />
              )}
            </IconButton>
          </Tooltip>
          <IconButton color="inherit" onClick={handleLogout} size="small">
            <LogoutIcon fontSize="small" />
          </IconButton>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default TopBar;
