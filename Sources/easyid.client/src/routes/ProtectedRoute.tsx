import React, { useEffect, useState, useCallback } from "react";
import { Navigate } from "react-router-dom";
import UsersService from "../services/usersService";
import { userStore } from "../stores/userStore";
import { Box, CircularProgress, Paper, Button, Typography, Stack } from "@mui/material";
import { useTranslation } from "react-i18next";

const ProtectedRoute: React.FC<{ children: React.ReactElement }> = ({
  children,
}) => {
  const [status, setStatus] = useState<"checking" | "allowed" | "denied-auth" | "error">("checking");
  const [errorStatus, setErrorStatus] = useState<number | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const { t } = useTranslation();

  const load = useCallback(() => {
    setStatus("checking");
    setErrorStatus(null);
    setErrorMessage(null);
    let mounted = true;
    UsersService.get("me")
      .then((user) => {
        if (!mounted) return;
        userStore.getState().setUser(user);
        setStatus("allowed");
      })
      .catch((e: unknown) => {
        if (!mounted) return;
        const err = e as Record<string, unknown> | null;
        let code: number | undefined;
        if (err && typeof err === "object" && "response" in err) {
          const r = (err as { response?: { status?: number; data?: unknown } }).response;
          code = r?.status;
        }
        if (code === 401 || code === 403) {
          setStatus("denied-auth");
          setErrorStatus(code || null);
          return;
        }
        setErrorStatus(code || null);
        // Decide message
        let msg: string;
        if (!code) msg = t("protectedRoute.errors.network");
        else if (code >= 500) msg = t("protectedRoute.errors.serverError");
        else msg = t("protectedRoute.errors.unexpected", { code });
        setErrorMessage(msg);
        setStatus("error");
      });
    return () => {
      mounted = false;
    };
  }, [t]);

  useEffect(() => {
    load();
  }, [load]);

  if (status === "checking")
    return (
      <Box
        display="flex"
        alignItems="center"
        justifyContent="center"
        height="100vh"
      >
        <CircularProgress />
      </Box>
    );
  if (status === "denied-auth") {
    return <Navigate to="/login" replace state={{ reason: "unauthorized", code: errorStatus }} />;
  }
  if (status === "error") {
    return (
      <Box display="flex" alignItems="center" justifyContent="center" height="100vh" p={2}>
        <Paper sx={{ maxWidth: 480, p: 3 }} elevation={4}>
          <Stack spacing={2}>
            <Typography variant="h6">{t("protectedRoute.errors.title")}</Typography>
            <Typography variant="body2" color="text.secondary">
              {errorMessage}
            </Typography>
            {errorStatus && (
              <Typography variant="caption" color="text.secondary">
                {t("protectedRoute.errors.code", { code: errorStatus })}
              </Typography>
            )}
            <Box display="flex" gap={1} justifyContent="flex-end">
              <Button variant="outlined" size="small" onClick={() => load()}>
                {t("protectedRoute.actions.retry")}
              </Button>
              <Button
                variant="contained"
                color="primary"
                size="small"
                onClick={() => (window.location.href = "/login")}
              >
                {t("protectedRoute.actions.goLogin")}
              </Button>
            </Box>
          </Stack>
        </Paper>
      </Box>
    );
  }
  return children;
};

export default ProtectedRoute;
