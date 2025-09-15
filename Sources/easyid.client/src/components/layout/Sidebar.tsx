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
  return (
    <List dense sx={{ width: "100%" }}>
      {sorted.map((it) => {
        const selected = location.pathname.startsWith(it.route);
        return (
          <ListItemButton
            key={it.key}
            selected={selected}
            onClick={() => {
              if (onNavigate) onNavigate(it.route);
              navigate(it.route);
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
