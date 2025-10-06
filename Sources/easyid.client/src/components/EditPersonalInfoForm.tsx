import { useTranslation } from "react-i18next";
import UsersService from "../services/usersService";
import React, { useCallback, useEffect, useMemo, useState } from "react";
import { Box, Button, Grid, Stack, TextField } from "@mui/material";
import { toast } from "react-toastify";

export interface PersonalInfoData {
  username?: string;
  firstName?: string;
  lastName?: string;
  middleName?: string;
}

interface EditPersonalInfoFormProps {
  username: string;
  initial?: PersonalInfoData;
  onSaved?: (data: PersonalInfoData) => void;
}

const EditPersonalInfoForm: React.FC<EditPersonalInfoFormProps> = ({
  username: initialUsername,
  initial,
  onSaved,
}) => {
  const { t } = useTranslation();
  const [username, setUsername] = useState(initialUsername ?? "");
  const [firstName, setFirstName] = useState(initial?.firstName ?? "");
  const [lastName, setLastName] = useState(initial?.lastName ?? "");
  const [middleName, setMiddleName] = useState(initial?.middleName ?? "");
  // Sync internal state if parent passes new initial values (e.g., after fetch)
  useEffect(() => {
    setUsername(initialUsername ?? "");
    setFirstName(initial?.firstName ?? "");
    setLastName(initial?.lastName ?? "");
    setMiddleName(initial?.middleName ?? "");
  }, [
    initialUsername,
    initial?.firstName,
    initial?.lastName,
    initial?.middleName,
  ]);
  const [saving, setSaving] = useState(false);
  const [, setError] = useState<string | null>(null); // error surfaced via toast only
  const [touched, setTouched] = useState({
    username: false,
    firstName: false,
    lastName: false,
    middleName: false,
  });

  // Username validation: ^[a-z][a-z0-9_-]*$ (3-64 chars)
  const usernameRegex = /^[a-z][a-z0-9_-]*$/;
  const validateUsername = (value: string): string | null => {
    if (!value || value.trim() === "") return "Username is required.";
    if (value.length < 3) return "Username must be at least 3 characters long.";
    if (value.length > 64) return "Username cannot exceed 64 characters.";
    if (!usernameRegex.test(value)) {
      return "Username must start with a lowercase letter and contain only lowercase letters, digits, hyphens, and underscores.";
    }
    return null;
  };

  // FirstName validation: required, 1-100 chars, not only whitespace
  const validateFirstName = (value: string): string | null => {
    if (!value || value.trim() === "") return "First name is required.";
    if (value.length > 100) return "First name cannot exceed 100 characters.";
    if (/^\s+$/.test(value)) return "First name cannot be only whitespace.";
    return null;
  };

  // LastName/MiddleName validation: optional, max 100 chars, not only whitespace if provided
  const validateOptionalName = (
    value: string,
    fieldName: string,
  ): string | null => {
    if (!value) return null; // optional
    if (value.length > 100) return `${fieldName} cannot exceed 100 characters.`;
    if (/^\s+$/.test(value)) return `${fieldName} cannot be only whitespace.`;
    return null;
  };

  const usernameError = touched.username ? validateUsername(username) : null;
  const firstNameError = touched.firstName
    ? validateFirstName(firstName)
    : null;
  const lastNameError = touched.lastName
    ? validateOptionalName(lastName, "Last name")
    : null;
  const middleNameError = touched.middleName
    ? validateOptionalName(middleName, "Middle name")
    : null;

  const hasValidationErrors = !!(
    usernameError ||
    firstNameError ||
    lastNameError ||
    middleNameError
  );

  const data: PersonalInfoData = useMemo(
    () => ({ username, firstName, lastName, middleName }),
    [username, firstName, lastName, middleName],
  );

  // Check if data has changed from initial values
  const hasChanges = useMemo(() => {
    return (
      username !== initialUsername ||
      firstName !== (initial?.firstName ?? "") ||
      lastName !== (initial?.lastName ?? "") ||
      middleName !== (initial?.middleName ?? "")
    );
  }, [username, initialUsername, firstName, lastName, middleName, initial]);

  const canSave = hasChanges && !hasValidationErrors;

  const handleSave = useCallback(async () => {
    // Mark all as touched to show validation errors
    setTouched({
      username: true,
      firstName: true,
      lastName: true,
      middleName: true,
    });

    // Check for validation errors
    const errors = [
      validateUsername(username),
      validateFirstName(firstName),
      validateOptionalName(lastName, "Last name"),
      validateOptionalName(middleName, "Middle name"),
    ].filter(Boolean);

    if (errors.length > 0) {
      toast.error(errors[0]!);
      return;
    }

    setSaving(true);
    setError(null);
    try {
      await UsersService.updatePersonalInfo(data);
      toast.success(t("components.editPersonal.messages.personalSaved"));
      if (onSaved) onSaved(data);
    } catch (e: unknown) {
      const err = e as Record<string, unknown>;
      const response = err?.response as Record<string, unknown> | undefined;
      const msg =
        (response?.data as string | undefined) ||
        (err?.message as string | undefined) ||
        "Unknown error";
      setError(msg);
      toast.error(msg);
    } finally {
      setSaving(false);
    }
    // Validators are inline but stable, data memo already includes all field dependencies
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [data, onSaved, t]);

  return (
    <Stack gap={2}>
      {/* Errors & success handled via toast notifications */}
      <TextField
        label={t("profile.fields.username")}
        value={username}
        size="small"
        onChange={(e) => setUsername(e.target.value)}
        onBlur={() => setTouched((prev) => ({ ...prev, username: true }))}
        error={!!usernameError}
        helperText={usernameError}
        autoComplete="off"
        required
      />
      <Grid container spacing={2}>
        <Grid item xs={12} md={4}>
          <TextField
            fullWidth
            label={t("profile.fields.firstName")}
            value={firstName}
            size="small"
            onChange={(e) => setFirstName(e.target.value)}
            onBlur={() => setTouched((prev) => ({ ...prev, firstName: true }))}
            error={!!firstNameError}
            helperText={firstNameError}
            autoComplete="given-name"
            required
          />
        </Grid>
        <Grid item xs={12} md={4}>
          <TextField
            fullWidth
            label={t("profile.fields.lastName")}
            value={lastName}
            size="small"
            onChange={(e) => setLastName(e.target.value)}
            onBlur={() => setTouched((prev) => ({ ...prev, lastName: true }))}
            error={!!lastNameError}
            helperText={lastNameError}
            autoComplete="family-name"
          />
        </Grid>
        <Grid item xs={12} md={4}>
          <TextField
            fullWidth
            label={t("profile.fields.middleName")}
            value={middleName}
            size="small"
            onChange={(e) => setMiddleName(e.target.value)}
            onBlur={() => setTouched((prev) => ({ ...prev, middleName: true }))}
            error={!!middleNameError}
            helperText={middleNameError}
            autoComplete="off"
            inputProps={{
              "data-form-type": "other",
              "data-lpignore": "true",
            }}
          />
        </Grid>
      </Grid>
      <Box display="flex" justifyContent="flex-end">
        <Button
          variant="contained"
          onClick={handleSave}
          disabled={saving || !canSave}
        >
          {saving
            ? t("components.editPersonal.actions.saving")
            : t("components.editPersonal.actions.savePersonal")}
        </Button>
      </Box>
    </Stack>
  );
};

export default EditPersonalInfoForm;
