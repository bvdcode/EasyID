import {
  AppBar,
  Box,
  IconButton,
  Toolbar,
  Typography,
  Tooltip,
  Avatar,
} from "@mui/material";
import React from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
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
  const { t } = useTranslation();
  const navigate = useNavigate();
  const user = userStore((s) => s.user);
  const navigateToProfile = () => {
    if (user) {
      navigate("/app/profile");
    }
  };

  const handleLogout = () => {
    authStore.getState().logout();
    userStore.getState().clear();
    if (onLogout) {
      onLogout();
    }
  };

  const { isDarkMode, toggleTheme } = useAppTheme();

  return (
    <AppBar position="static" color="primary" elevation={1}>
      <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
        <Typography variant="h6" component="div">
          {title}
        </Typography>
        <Box display="flex" alignItems="center" gap={2}>
          <Tooltip
            title={
              isDarkMode
                ? t("settings.switchToLight")
                : t("settings.switchToDark")
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
          {user && (
            <Tooltip title={user.username}>
              <IconButton
                onClick={navigateToProfile}
                size="small"
                sx={{ p: 0, border: "1px solid rgba(255,255,255,0.3)" }}
              >
                <Avatar
                  src={user.avatarUrl}
                  alt={user.username}
                  sx={{ width: 24, height: 24, fontSize: 14 }}
                >
                  {user.username.charAt(0).toUpperCase()}
                </Avatar>
              </IconButton>
            </Tooltip>
          )}
          <Tooltip title={t("topBar.logout")}>
            <IconButton color="inherit" onClick={handleLogout} size="small">
              <LogoutIcon fontSize="small" />
            </IconButton>
          </Tooltip>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default TopBar;
