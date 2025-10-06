import React, { useCallback, useMemo, useState } from "react";
import { Alert, Box, Button, Stack, TextField } from "@mui/material";
import { useTranslation } from "react-i18next";
import UsersService from "../services/usersService";
import { toast } from "react-toastify";

interface ChangePasswordFormProps {
  onSuccess?: () => void;
}

const ChangePasswordForm: React.FC<ChangePasswordFormProps> = ({ onSuccess }) => {
  const { t } = useTranslation();
  const [oldPassword, setOldPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [repeatNewPassword, setRepeatNewPassword] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);
  const [touched, setTouched] = useState({ old: false, next: false, repeat: false });
  const [submitAttempted, setSubmitAttempted] = useState(false);

  const errors = useMemo(() => {
    const e: { old?: string; next?: string; repeat?: string } = {};
    if (!oldPassword) e.old = t("components.changePassword.validation.oldPasswordRequired");
    if (!newPassword) e.next = t("components.changePassword.validation.newPasswordRequired");
    else if (newPassword.length < 8) e.next = t("components.changePassword.validation.newPasswordMinLength");
    if (repeatNewPassword !== newPassword) e.repeat = t("components.changePassword.validation.passwordsDoNotMatch");
    return e;
  }, [oldPassword, newPassword, repeatNewPassword, t]);

  const hasErrors = Boolean(errors.old || errors.next || errors.repeat);
  const canSubmit = !hasErrors && !submitting;

  const handleSubmit = useCallback(async () => {
    setSubmitAttempted(true);
    if (!canSubmit) return;
    setSubmitting(true);
    setError(null);
    setSuccess(false);
    try {
      await UsersService.changePassword(oldPassword, newPassword);
      setSuccess(true);
      setOldPassword("");
      setNewPassword("");
      setRepeatNewPassword("");
      setTouched({ old: false, next: false, repeat: false });
      setSubmitAttempted(false);
      if (onSuccess) onSuccess();
    } catch (e: unknown) {
      const err = e as Record<string, unknown>;
      const response = (err?.response as Record<string, unknown> | undefined);
      const data = response?.data as unknown;
      const message = (err?.message as string | undefined) ?? (typeof data === 'string' ? data : undefined);
      const errorMsg = message ?? "Unknown error";
      setError(errorMsg);
      toast.error(errorMsg);
    } finally {
      setSubmitting(false);
    }
  }, [canSubmit, newPassword, oldPassword, onSuccess]);

  return (
    <Stack gap={2}>
  {error && <Alert severity="error">{error}</Alert>}
  {success && <Alert severity="success">{t("components.changePassword.messages.passwordChanged")}</Alert>}
      <TextField
        label={t("profile.fields.oldPassword")}
        type="password"
        size="small"
        value={oldPassword}
        onChange={(e) => setOldPassword(e.target.value)}
        onBlur={() => setTouched((s) => ({ ...s, old: true }))}
        autoComplete="current-password"
        error={(touched.old || submitAttempted) && Boolean(errors.old)}
        helperText={(touched.old || submitAttempted) ? (errors.old ?? "") : ""}
      />
      <TextField
        label={t("profile.fields.newPassword")}
        type="password"
        size="small"
        value={newPassword}
        onChange={(e) => setNewPassword(e.target.value)}
        onBlur={() => setTouched((s) => ({ ...s, next: true }))}
        autoComplete="new-password"
        error={(touched.next || submitAttempted) && Boolean(errors.next)}
        helperText={(touched.next || submitAttempted) ? (errors.next ?? "") : ""}
      />
      <TextField
        label={t("profile.fields.repeatNewPassword")}
        type="password"
        size="small"
        value={repeatNewPassword}
        onChange={(e) => setRepeatNewPassword(e.target.value)}
        onBlur={() => setTouched((s) => ({ ...s, repeat: true }))}
        autoComplete="new-password"
        error={(touched.repeat || submitAttempted) && Boolean(errors.repeat)}
        helperText={(touched.repeat || submitAttempted) ? (errors.repeat ?? "") : ""}
      />
      <Box display="flex" justifyContent="flex-end">
        <Button variant="contained" onClick={handleSubmit} disabled={!canSubmit}>
          {submitting ? t("components.changePassword.actions.saving") : t("components.changePassword.actions.changePassword")}
        </Button>
      </Box>
    </Stack>
  );
};

export default ChangePasswordForm;
