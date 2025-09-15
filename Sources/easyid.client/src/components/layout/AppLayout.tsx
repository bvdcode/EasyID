import TopBar from "./TopBar";
import React, { useEffect } from "react";
import Sidebar, { SidebarItem } from "./Sidebar";
import { userStore } from "../../stores/userStore";
import { Box, Divider, Paper } from "@mui/material";
import { Outlet, useNavigate } from "react-router-dom";

interface AppLayoutProps {
  sidebarItems: SidebarItem[];
  title?: string;
}

const AppLayout: React.FC<AppLayoutProps> = ({ sidebarItems, title }) => {
  const fetchUser = userStore((s) => s.fetchUser);
  const user = userStore((s) => s.user);
  const navigate = useNavigate();

  useEffect(() => {
    if (!user) fetchUser();
  }, [user, fetchUser]);

  return (
    <Box display="flex" flexDirection="column" height="100vh" width="100%">
      <TopBar title={title} onLogout={() => navigate("/login", { replace: true })} />
      <Box display="flex" flex={1} overflow="hidden">
        <Paper
          elevation={0}
          sx={{
            width: 240,
            borderRight: 1,
            borderColor: "divider",
            overflowY: "auto",
            display: "flex",
            flexDirection: "column",
          }}
        >
          <Sidebar items={sidebarItems} />
        </Paper>
        <Divider flexItem orientation="vertical" />
        <Box
          component="main"
          sx={{ flex: 1, p: 2, overflowY: "auto", backgroundColor: "background.default" }}
        >
          <Outlet />
        </Box>
      </Box>
    </Box>
  );
};

export default AppLayout;
