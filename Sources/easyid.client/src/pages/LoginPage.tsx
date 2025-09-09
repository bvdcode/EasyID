import {
  Alert,
  Box,
  Button,
  Paper,
  TextField,
  Typography,
} from "@mui/material";
import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { MetricsService } from "../services";

const LoginPage: React.FC = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const [username, setUsername] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [hasUsers, setHasUsers] = useState<boolean | null>(null);

  useEffect(() => {
    MetricsService.hasUsers()
      .then(setHasUsers)
      .catch(() => setHasUsers(true)); // if API fails, assume users exist to avoid misleading message
  }, []);

  const handleLogin = () => {
    if (!password) {
      alert(t("loginPage.emptyPasswordError"));
      return;
    }
    // For now, keep existing behavior: navigate to vault with password as state
    // Backend auth endpoints are wired but not used yet because Vault API expects password
    navigate("/vault", { state: { password } });
  };

  return (
    <Paper
      elevation={3}
      style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        userSelect: "none",
      }}
    >
      <Box
        display="flex"
        flexDirection="column"
        gap={2}
        maxWidth={480}
        padding={2}
      >
        <Typography variant="h6">{t("loginPage.loginTitle")}</Typography>
        <TextField
          id="username-input"
          label={t("loginPage.usernameLabel")}
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          fullWidth
          variant="outlined"
          autoComplete="username"
        />
        <TextField
          id="password-input"
          label={t("loginPage.passwordTitle")}
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          fullWidth
          variant="outlined"
          type="password"
          autoComplete="current-password"
        />
        {hasUsers === false && (
          <Alert severity="info">{t("loginPage.firstUserInfo")}</Alert>
        )}
        <Button
          variant="contained"
          color="primary"
          onClick={handleLogin}
          disabled={!password}
        >
          {t("loginPage.loginButton")}
        </Button>
      </Box>
    </Paper>
  );
};

export default LoginPage;
