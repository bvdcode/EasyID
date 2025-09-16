import {
  Typography,
  Paper,
  Stack,
  Box,
  TextField,
  Button,
} from "@mui/material";
import { userStore } from "../stores/userStore";
import React, { useState } from "react";
import AvatarUploader from "../components/AvatarUploader";
import { useTranslation } from "react-i18next";

const ProfilePage: React.FC = () => {
  const user = userStore((s) => s.user);
  const { t } = useTranslation();

  // Local form state (backend integration TBD)
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [middleName, setMiddleName] = useState("");
  const [oldPassword, setOldPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [repeatNewPassword, setRepeatNewPassword] = useState("");

  const canSave = true; // placeholder; add validation later
  return (
    <Paper sx={{ p: 3, display: "flex", flexDirection: "column", gap: 4 }}>
      <Typography variant="h5" gutterBottom>
        {t("profile.title")}
      </Typography>
      {user ? (
        <Stack spacing={4}>
          <Stack
            direction={{ xs: "column", md: "row" }}
            spacing={4}
            alignItems="flex-start"
          >
            <AvatarUploader />
            <Paper
              variant="outlined"
              sx={{
                p: 3,
                flex: 1,
                display: "flex",
                flexDirection: "column",
                gap: 2,
              }}
            >
              <Typography variant="h6" gutterBottom>
                {t("profile.sections.personal")}
              </Typography>
              <TextField
                label={t("profile.fields.username")}
                value={user.username}
                size="small"
                InputProps={{ readOnly: true }}
              />
              <TextField
                label={t("profile.fields.firstName")}
                value={firstName}
                size="small"
                onChange={(e) => setFirstName(e.target.value)}
              />
              <TextField
                label={t("profile.fields.lastName")}
                value={lastName}
                size="small"
                onChange={(e) => setLastName(e.target.value)}
              />
              <TextField
                label={t("profile.fields.middleName")}
                value={middleName}
                size="small"
                onChange={(e) => setMiddleName(e.target.value)}
              />
            </Paper>
          </Stack>
          <Paper
            variant="outlined"
            sx={{ p: 3, display: "flex", flexDirection: "column", gap: 2 }}
          >
            <Typography variant="h6" gutterBottom>
              {t("profile.sections.security")}
            </Typography>
            <TextField
              label={t("profile.fields.oldPassword")}
              type="password"
              size="small"
              value={oldPassword}
              onChange={(e) => setOldPassword(e.target.value)}
              autoComplete="current-password"
            />
            <TextField
              label={t("profile.fields.newPassword")}
              type="password"
              size="small"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              autoComplete="new-password"
            />
            <TextField
              label={t("profile.fields.repeatNewPassword")}
              type="password"
              size="small"
              value={repeatNewPassword}
              onChange={(e) => setRepeatNewPassword(e.target.value)}
              autoComplete="new-password"
            />
          </Paper>
          <Box display="flex" justifyContent="flex-end">
            <Button variant="contained" disabled={!canSave}>
              {t("profile.actions.save")}
            </Button>
          </Box>
        </Stack>
      ) : (
        <Typography variant="body2" color="text.secondary">
          {t("profile.messages.noUser")}
        </Typography>
      )}
    </Paper>
  );
};

export default ProfilePage;
