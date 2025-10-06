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
    <Paper
      sx={{
        p: { xs: 2, md: 3 },
        display: "flex",
        flexDirection: "column",
        gap: 3,
      }}
    >
      <Typography variant="h5" gutterBottom>
        {t("profile.title")}
      </Typography>
      {user ? (
        <Stack spacing={3} alignItems="center">
          <Box
            sx={{
              width: "100%",
              maxWidth: 980,
              display: "flex",
              flexDirection: "column",
              gap: 3,
            }}
          >
            <Grid container spacing={2} alignItems="flex-start">
              <Grid item xs={12} md={3} lg={2} sx={{ display: "flex", justifyContent: { xs: "center", md: "flex-start" } }}>
                <AvatarUploader />
              </Grid>
              <Grid item xs={12} md={9} lg={10}>
                <Paper
                  variant="outlined"
                  sx={{ p: { xs: 2, md: 3 }, height: "100%", display: "flex", flexDirection: "column" }}
                >
                  <Typography variant="h6" gutterBottom>
                    {t("profile.sections.personal")}
                  </Typography>
                  <EditPersonalInfoForm username={user.username} />
                </Paper>
              </Grid>
            </Grid>
            <Paper
              variant="outlined"
              sx={{ p: { xs: 2, md: 3 }, maxWidth: 980, alignSelf: "stretch" }}
            >
              <Typography variant="h6" gutterBottom>
                {t("profile.sections.security")}
              </Typography>
              <ChangePasswordForm />
            </Paper>
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
