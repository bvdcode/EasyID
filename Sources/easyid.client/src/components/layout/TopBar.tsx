import React from "react";
import { AppBar, Box, IconButton, Toolbar, Typography } from "@mui/material";
import LogoutIcon from "@mui/icons-material/Logout";
import { authStore } from "../../stores/authStore";
import { userStore } from "../../stores/userStore";

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
          <IconButton color="inherit" onClick={handleLogout} size="small">
            <LogoutIcon fontSize="small" />
          </IconButton>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default TopBar;
