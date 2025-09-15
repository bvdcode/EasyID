import {
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
} from "@mui/material";
import React from "react";
import { useNavigate, useLocation } from "react-router-dom";

export interface SidebarItem {
  key: string;
  label: string;
  icon?: React.ReactNode;
  route: string;
  order?: number;
}

interface SidebarProps {
  items: SidebarItem[];
  onNavigate?: (route: string) => void;
}

const Sidebar: React.FC<SidebarProps> = ({ items, onNavigate }) => {
  const navigate = useNavigate();
  const location = useLocation();
  const sorted = [...items].sort((a, b) => (a.order ?? 0) - (b.order ?? 0));

  // Determine active item: prefer exact match; otherwise longest prefix match
  const path = location.pathname;
  let activeKey: string | null = null;
  // exact
  const exact = sorted.find((i) => i.route === path);
  if (exact) activeKey = exact.key;
  else {
    // longest prefix (skip root collisions like '/app' vs '/app/profile/edit')
    let bestLen = -1;
    for (const i of sorted) {
      if (path.startsWith(i.route) && i.route.length > bestLen) {
        bestLen = i.route.length;
        activeKey = i.key;
      }
    }
  }

  return (
    <List dense sx={{ width: "100%" }}>
      {sorted.map((it) => {
        const selected = it.key === activeKey;
        return (
          <ListItemButton
            key={it.key}
            selected={selected}
            onClick={() => {
              if (onNavigate) onNavigate(it.route);
              if (path !== it.route) navigate(it.route);
            }}
          >
            {it.icon && <ListItemIcon>{it.icon}</ListItemIcon>}
            <ListItemText primary={it.label} />
          </ListItemButton>
        );
      })}
    </List>
  );
};

export default Sidebar;
