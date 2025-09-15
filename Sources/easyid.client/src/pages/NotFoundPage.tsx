import React from "react";
import { Box, Button, Paper, Stack, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

const NotFoundPage: React.FC = () => {
  const navigate = useNavigate();
  const { t } = useTranslation();

  return (
    <Box display="flex" justifyContent="center" alignItems="center" height="100%">
      <Paper sx={{ p: 4, maxWidth: 520 }} elevation={2}>
        <Stack spacing={2}>
          <Typography variant="h4" component="h1">
            404
          </Typography>
          <Typography variant="h6">{t("notFound.title")}</Typography>
          <Typography variant="body2" color="text.secondary">
            {t("notFound.description")}
          </Typography>
          <Box>
            <Button
              variant="contained"
              color="primary"
              onClick={() => navigate("/app", { replace: true })}
            >
              {t("notFound.goHome")}
            </Button>
          </Box>
        </Stack>
      </Paper>
    </Box>
  );
};

export default NotFoundPage;
