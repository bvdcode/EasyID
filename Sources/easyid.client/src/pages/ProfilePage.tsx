import { Typography, Paper, Stack, Box, Grid } from "@mui/material";
import { userStore } from "../stores/userStore";
import React from "react";
import AvatarUploader from "../components/AvatarUploader";
import { useTranslation } from "react-i18next";
import ChangePasswordForm from "../components/ChangePasswordForm";
import EditPersonalInfoForm from "../components/EditPersonalInfoForm";

const ProfilePage: React.FC = () => {
  const user = userStore((s) => s.user);
  const { t } = useTranslation();

  // Local form state (backend integration TBD)
  // Personal info form is encapsulated in EditPersonalInfoForm

  // const canSave = true; // placeholder; add validation later
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
                <Typography variant="h6" gutterBottom>
                  {t("profile.sections.personal")}
                </Typography>
                <EditPersonalInfoForm username={user.username} />
              </Paper>
            </Grid>
          </Grid>

          <Paper variant="outlined" sx={{ p: { xs: 2, md: 3 } }}>
            <Typography variant="h6" gutterBottom>
              {t("profile.sections.security")}
            </Typography>
            <ChangePasswordForm />
          </Paper>
          {/* Page-level save removed; each form owns its Save button */}
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
