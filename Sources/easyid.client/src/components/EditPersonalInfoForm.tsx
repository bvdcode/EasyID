import React, { useCallback, useMemo, useState } from "react";
import { Alert, Box, Button, Grid, Stack, TextField } from "@mui/material";
import { useTranslation } from "react-i18next";
import UsersService from "../services/usersService";

export interface PersonalInfoData {
  firstName?: string;
  lastName?: string;
  middleName?: string;
}

interface EditPersonalInfoFormProps {
  username: string;
  initial?: PersonalInfoData;
  onSaved?: (data: PersonalInfoData) => void;
}

const EditPersonalInfoForm: React.FC<EditPersonalInfoFormProps> = ({ username, initial, onSaved }) => {
  const { t } = useTranslation();
  const [firstName, setFirstName] = useState(initial?.firstName ?? "");
  const [lastName, setLastName] = useState(initial?.lastName ?? "");
  const [middleName, setMiddleName] = useState(initial?.middleName ?? "");
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const data: PersonalInfoData = useMemo(() => ({ firstName, lastName, middleName }), [firstName, lastName, middleName]);

  const handleSave = useCallback(async () => {
    setSaving(true);
    setError(null);
    setSuccess(false);
    try {
      // Attempt to save; backend endpoint may be introduced later
      await UsersService.updatePersonalInfo(data);
      setSuccess(true);
      if (onSaved) onSaved(data);
    } catch (e: unknown) {
      const err = e as Record<string, unknown>;
      const response = err?.response as Record<string, unknown> | undefined;
      const msg = (response?.data as string | undefined) || (err?.message as string | undefined) || "Unknown error";
      setError(msg);
    } finally {
      setSaving(false);
    }
  }, [data, onSaved]);

  return (
    <Stack gap={2}>
  {error && <Alert severity="error">{error}</Alert>}
  {success && <Alert severity="success">{t("components.editPersonal.messages.personalSaved")}</Alert>}
      <TextField
        label={t("profile.fields.username")}
        value={username}
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
      <Box display="flex" justifyContent="flex-end">
        <Button variant="contained" onClick={handleSave} disabled={saving}>
          {saving ? t("components.editPersonal.actions.saving") : t("components.editPersonal.actions.savePersonal")}
        </Button>
      </Box>
    </Stack>
  );
};

export default EditPersonalInfoForm;
