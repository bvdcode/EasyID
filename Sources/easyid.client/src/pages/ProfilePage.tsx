import React from "react";
import { Box, Typography, Paper } from "@mui/material";
import { userStore } from "../stores/userStore";

const ProfilePage: React.FC = () => {
  const user = userStore((s) => s.user);

  return (
    <Paper sx={{ p: 3 }}>
      <Typography variant="h5" gutterBottom>
        Profile
      </Typography>
      {user ? (
        <Box display="flex" flexDirection="column" gap={1}>
          <Typography>ID: {user.id}</Typography>
          <Typography>Username: {user.username}</Typography>
          <Typography>Email: {user.email}</Typography>
          {user.avatarUrl && (
            <img
              src={user.avatarUrl}
              alt="avatar"
              style={{ width: 96, height: 96, borderRadius: "50%" }}
            />
          )}
        </Box>
      ) : (
        <Typography variant="body2" color="text.secondary">
          No user loaded
        </Typography>
      )}
    </Paper>
  );
};

export default ProfilePage;
