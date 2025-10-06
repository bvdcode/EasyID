import { Alert, Box, Button, Paper, TextField, Typography } from "@mui/material";
import { toast } from "react-toastify";
import { MetricsService } from "../services";
import { useTranslation } from "react-i18next";
import { useNavigate, useLocation } from "react-router-dom";
import { authStore } from "../stores/authStore";
import { userStore } from "../stores/userStore";
import React, { useEffect, useState } from "react";

const LoginPage: React.FC = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const [username, setUsername] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [hasUsers, setHasUsers] = useState<boolean | null>(null);
  const location = useLocation();

  useEffect(() => {
    MetricsService.getMetrics()
      .then((data) => {
        if (data) {
          setHasUsers(data.hasUsers);
        }
      })
      .catch(() => setHasUsers(true)); // if API fails, assume users exist to avoid misleading message
  }, []);

  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const routeState = (location.state as { reason?: string; code?: number } | null) || null;
  const initialRouteError = React.useMemo(() => {
    if (!routeState?.reason) return null;
    switch (routeState.reason) {
      case "unauthorized":
        return t("authReasons.unauthorized");
      case "serverError":
        return t("loginErrors.serverError");
      case "unexpected":
        return t("loginErrors.unexpected");
      default:
        return null;
    }
  }, [routeState?.reason, t]);

  const handleLogin = async () => {
    setError(null);
    if (!username || !password) {
      toast.warn(t("loginPage.emptyPasswordError"));
      return;
    }
    setSubmitting(true);
    try {
  await authStore.getState().login(username, password);
  await userStore.getState().fetchUser();
  navigate("/app", { replace: true });
    } catch (e: unknown) {
      const status =
        typeof e === "object" && e !== null && "response" in e
          ? // eslint-disable-next-line @typescript-eslint/no-explicit-any
            (e as Record<string, any>).response?.status
          : undefined;
      if (status === 401) {
        const msg = t("loginErrors.invalidCredentials", { defaultValue: "Invalid username or password" });
        setError(msg);
        toast.error(msg);
      } else {
        const msg = (e as Error)?.message || t("loginErrors.unexpected", { defaultValue: "Login failed" });
        setError(msg);
        toast.error(msg);
      }
    } finally {
      setSubmitting(false);
    }
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
        {(error || initialRouteError) && (
          <Alert severity="error">{error || initialRouteError}</Alert>
        )}
        <Button
          variant="contained"
          color="primary"
          onClick={handleLogin}
          disabled={!username || !password || submitting}
        >
          {t("loginPage.loginButton")}
        </Button>
      </Box>
    </Paper>
  );
};

export default LoginPage;
