import { Typography, Paper, Stack, Box, TextField, Button, Grid } from "@mui/material";
import { userStore } from "../stores/userStore";
import React, { useState } from "react";
import AvatarUploader from "../components/AvatarUploader";
import { useTranslation } from "react-i18next";
import ChangePasswordForm from "../components/ChangePasswordForm";

const ProfilePage: React.FC = () => {
  const user = userStore((s) => s.user);
  const { t } = useTranslation();

  // Local form state (backend integration TBD)
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [middleName, setMiddleName] = useState("");

  const canSave = true; // placeholder; add validation later
  return (
    <Paper sx={{ p: { xs: 2, md: 3 }, display: "flex", flexDirection: "column", gap: 3 }}>
      <Typography variant="h5" gutterBottom>
        {t("profile.title")}
      </Typography>
      {user ? (
        <Stack spacing={3}>
          <Grid container spacing={2} alignItems="stretch">
            <Grid item xs={12} md="auto">
              <Box sx={{ display: "flex", justifyContent: { xs: "center", md: "flex-start" } }}>
                <AvatarUploader />
              </Box>
            </Grid>
            <Grid item xs={12} md>
              <Paper variant="outlined" sx={{ p: { xs: 2, md: 3 } }}>
                <Stack gap={2}>
                  <Typography variant="h6" gutterBottom>
                    {t("profile.sections.personal")}
                  </Typography>
                  <TextField
                    label={t("profile.fields.username")}
                    value={user.username}
                    size="small"
                    InputProps={{ readOnly: true }}
                  />
                  <Grid container spacing={2}>
                    <Grid item xs={12} md={4}>
                      <TextField
                        fullWidth
                        label={t("profile.fields.firstName")}
                        value={firstName}
                        size="small"
                        onChange={(e) => setFirstName(e.target.value)}
                      />
                    </Grid>
                    <Grid item xs={12} md={4}>
                      <TextField
                        fullWidth
                        label={t("profile.fields.lastName")}
                        value={lastName}
                        size="small"
                        onChange={(e) => setLastName(e.target.value)}
                      />
                    </Grid>
                    <Grid item xs={12} md={4}>
                      <TextField
                        fullWidth
                        label={t("profile.fields.middleName")}
                        value={middleName}
                        size="small"
                        onChange={(e) => setMiddleName(e.target.value)}
                      />
                    </Grid>
                  </Grid>
                </Stack>
              </Paper>
            </Grid>
          </Grid>

          <Paper variant="outlined" sx={{ p: { xs: 2, md: 3 } }}>
            <Typography variant="h6" gutterBottom>
              {t("profile.sections.security")}
            </Typography>
            <ChangePasswordForm />
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
