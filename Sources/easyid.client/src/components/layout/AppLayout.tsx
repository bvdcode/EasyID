import TopBar from "./TopBar";
import { useTheme } from "@mui/material/styles";
import React, { useEffect, useMemo } from "react";
import Sidebar, { SidebarItem } from "./Sidebar";
import { userStore } from "../../stores/userStore";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { Box, Divider, Paper, Tabs, Tab, useMediaQuery } from "@mui/material";

interface AppLayoutProps {
  sidebarItems: SidebarItem[];
  title?: string;
}

const AppLayout: React.FC<AppLayoutProps> = ({ sidebarItems, title }) => {
  const fetchUser = userStore((s) => s.fetchUser);
  const user = userStore((s) => s.user);
  const navigate = useNavigate();
  const location = useLocation();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));

  const sorted = useMemo(
    () => [...sidebarItems].sort((a, b) => (a.order ?? 0) - (b.order ?? 0)),
    [sidebarItems],
  );

  const activeKey = useMemo(() => {
    const path = location.pathname;
    // exact match
    const exact = sorted.find((i) => i.route === path);
    if (exact) return exact.key;
    // longest prefix match
    let bestLen = -1;
    let key: string | null = null;
    for (const i of sorted) {
      if (path.startsWith(i.route) && i.route.length > bestLen) {
        bestLen = i.route.length;
        key = i.key;
      }
    }
    return key;
  }, [location.pathname, sorted]);

  useEffect(() => {
    if (!user) fetchUser();
  }, [user, fetchUser]);

  return (
    <Box display="flex" flexDirection="column" height="100vh" width="100%">
      <TopBar
        title={title}
        onLogout={() => navigate("/login", { replace: true })}
      />
      {isMobile && (
        <Paper elevation={0} sx={{ borderBottom: 1, borderColor: "divider" }}>
          <Tabs
            value={activeKey ?? false}
            onChange={(_e, value: string) => {
              const target = sorted.find((i) => i.key === value);
              if (target && location.pathname !== target.route) {
                navigate(target.route);
              }
            }}
            variant="scrollable"
            scrollButtons="auto"
            aria-label="navigation tabs"
          >
            {sorted.map((it) => (
              <Tab key={it.key} value={it.key} label={it.label} />
            ))}
          </Tabs>
        </Paper>
      )}
      <Box display="flex" flex={1} overflow="hidden">
        {!isMobile && (
          <>
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
          </>
        )}
        <Box
          component="main"
          sx={{
            flex: 1,
            p: 2,
            overflowY: "auto",
            backgroundColor: "background.default",
          }}
        >
          <Outlet />
        </Box>
      </Box>
    </Box>
  );
};

export default AppLayout;
