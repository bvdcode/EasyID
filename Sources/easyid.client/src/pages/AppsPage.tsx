import React, { useState } from "react";
import {
  Box,
  Button,
  Card,
  CardContent,
  IconButton,
  Stack,
  TextField,
  Tooltip,
  Typography,
  InputAdornment,
} from "@mui/material";
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  History as HistoryIcon,
  ContentCopy as CopyIcon,
} from "@mui/icons-material";
import { toast } from "react-toastify";

interface AppData {
  id: string;
  name: string;
  description: string;
  clientId: string;
  clientSecret: string;
  logo?: string;
}

const AppsPage: React.FC = () => {
  // Mock data
  const [apps] = useState<AppData[]>([
    {
      id: "1",
      name: "Mobile App",
      description: "iOS and Android application for end users",
      clientId: "mobile_app_client_123456",
      clientSecret: "secret_abc123xyz789def456ghi",
      logo: "📱",
    },
    {
      id: "2",
      name: "Web Dashboard",
      description: "Admin dashboard for managing users and permissions",
      clientId: "web_dashboard_client_789012",
      clientSecret: "secret_xyz789abc123def456ghi",
      logo: "🌐",
    },
    {
      id: "3",
      name: "API Integration",
      description: "Third-party API integration service",
      clientId: "api_integration_client_345678",
      clientSecret: "secret_def456ghi789abc123xyz",
      logo: "🔗",
    },
  ]);

  const handleCopy = (text: string, label: string) => {
    navigator.clipboard.writeText(text).then(() => {
      toast.success(`${label} copied to clipboard`);
    });
  };

  const handleEdit = (appId: string) => {
    toast.info(`Edit app ${appId} - to be implemented`);
  };

  const handleDelete = (appId: string) => {
    toast.info(`Delete app ${appId} - to be implemented`);
  };

  const handleHistory = (appId: string) => {
    toast.info(`Activity log for app ${appId} - to be implemented`);
  };

  const handleAddNew = () => {
    toast.info("Add new application - to be implemented");
  };

  return (
    <Box p={3}>
      <Stack spacing={3}>
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Typography variant="h4">Applications</Typography>
        </Box>

        {apps.map((app) => (
          <Card key={app.id} elevation={2} sx={{ borderRadius: 2 }}>
            <CardContent>
              <Box display="flex" gap={3} alignItems="flex-start">
                {/* Logo */}
                <Box
                  sx={{
                    fontSize: "3rem",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    minWidth: 80,
                    minHeight: 80,
                    bgcolor: "action.hover",
                    borderRadius: 2,
                  }}
                >
                  {app.logo || "📦"}
                </Box>

                {/* Content */}
                <Box flex={1}>
                  <Stack spacing={2}>
                    <Box>
                      <Typography variant="h6" gutterBottom>
                        {app.name}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        {app.description}
                      </Typography>
                    </Box>

                    {/* Client ID */}
                    <TextField
                      label="Client ID"
                      value={app.clientId}
                      size="small"
                      InputProps={{
                        readOnly: true,
                        endAdornment: (
                          <InputAdornment position="end">
                            <Tooltip title="Copy Client ID">
                              <IconButton
                                edge="end"
                                onClick={() =>
                                  handleCopy(app.clientId, "Client ID")
                                }
                                size="small"
                              >
                                <CopyIcon fontSize="small" />
                              </IconButton>
                            </Tooltip>
                          </InputAdornment>
                        ),
                      }}
                      fullWidth
                    />

                    {/* Client Secret */}
                    <TextField
                      label="Client Secret"
                      value={app.clientSecret}
                      size="small"
                      type="password"
                      InputProps={{
                        readOnly: true,
                        endAdornment: (
                          <InputAdornment position="end">
                            <Tooltip title="Copy Client Secret">
                              <IconButton
                                edge="end"
                                onClick={() =>
                                  handleCopy(app.clientSecret, "Client Secret")
                                }
                                size="small"
                              >
                                <CopyIcon fontSize="small" />
                              </IconButton>
                            </Tooltip>
                          </InputAdornment>
                        ),
                      }}
                      fullWidth
                    />
                  </Stack>
                </Box>

                {/* Actions */}
                <Box display="flex" flexDirection="column" gap={1}>
                  <Tooltip title="Edit">
                    <IconButton
                      color="primary"
                      onClick={() => handleEdit(app.id)}
                    >
                      <EditIcon />
                    </IconButton>
                  </Tooltip>
                  <Tooltip title="Activity Log">
                    <IconButton
                      color="info"
                      onClick={() => handleHistory(app.id)}
                    >
                      <HistoryIcon />
                    </IconButton>
                  </Tooltip>
                  <Tooltip title="Delete">
                    <IconButton
                      color="error"
                      onClick={() => handleDelete(app.id)}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </Tooltip>
                </Box>
              </Box>
            </CardContent>
          </Card>
        ))}

        {/* Add New Button */}
        <Box display="flex" justifyContent="center" mt={2}>
          <Button
            variant="contained"
            size="large"
            startIcon={<AddIcon />}
            onClick={handleAddNew}
            sx={{ borderRadius: 3, px: 4, py: 1.5 }}
          >
            Add New Application
          </Button>
        </Box>
      </Stack>
    </Box>
  );
};

export default AppsPage;
