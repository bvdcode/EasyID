import React from "react";
import { Box, Typography } from "@mui/material";

const DashboardPage: React.FC = () => {
  return (
    <Box display="flex" flexDirection="column" gap={2}>
      <Typography variant="h5">Dashboard</Typography>
      <Typography variant="body2" color="text.secondary">
        Welcome. Replace this placeholder with real widgets / stats.
      </Typography>
    </Box>
  );
};

export default DashboardPage;
