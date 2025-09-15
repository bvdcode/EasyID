import { Typography, Paper, Stack } from "@mui/material";
import { userStore } from "../stores/userStore";
import React from "react";
import AvatarUploader from "../components/AvatarUploader";

const ProfilePage: React.FC = () => {
  const user = userStore((s) => s.user);
  return (
    <Paper sx={{ p: 3 }}>
      <Typography variant="h5" gutterBottom>
        Profile
      </Typography>
      {user ? (
        <Stack
          direction={{ xs: "column", sm: "row" }}
          spacing={4}
          alignItems="flex-start"
        >
          <AvatarUploader />
          <Stack spacing={1} flex={1} minWidth={240}>
            <Typography>ID: {user.id}</Typography>
            <Typography>Username: {user.username}</Typography>
            <Typography>Email: {user.email}</Typography>
          </Stack>
        </Stack>
      ) : (
        <Typography variant="body2" color="text.secondary">
          No user loaded
        </Typography>
      )}
    </Paper>
  );
};

export default ProfilePage;
