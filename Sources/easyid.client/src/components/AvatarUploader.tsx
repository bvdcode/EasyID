import {
  Stack,
  Avatar,
  Button,
  Typography,
  CircularProgress,
  Box,
} from "@mui/material";
import { userStore } from "../stores/userStore";
import UsersService from "../services/usersService";
import React, { useCallback, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";

interface AvatarUploaderProps {
  size?: number;
  variant?: "circular" | "rounded" | "square";
}

// Single Responsibility: handles avatar display + upload interaction only
const AvatarUploader: React.FC<AvatarUploaderProps> = ({
  size = 120,
  variant = "circular",
}) => {
  const user = userStore((s) => s.user);
  const fetchUser = userStore((s) => s.fetchUser);
  const fileRef = useRef<HTMLInputElement | null>(null);
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [version, setVersion] = useState(0);
  const { t } = useTranslation();
  const navigate = useNavigate();

  // avatar endpoint is public (no auth) so always use concrete id, not "me"
  const baseUrl = user ? UsersService.avatarUrl(user.id) : "";
  const src = user ? `${baseUrl}?v=${version}` : undefined; // version busts browser cache after upload

  const pick = () => fileRef.current?.click();

  const onChange = useCallback(
    async (e: React.ChangeEvent<HTMLInputElement>) => {
      const file = e.target.files?.[0];
      if (!file) return;
      setError(null);
      setUploading(true);
      try {
        await UsersService.updateAvatar(file);
        // Refresh user (even though avatarUrl removed, may need future fields)
        await fetchUser();
        setVersion((v) => v + 1); // cache-bust
      } catch (err) {
        const error = err as { response?: { status?: number } };
        const status = error.response?.status;

        // Redirect to login on auth errors
        if (status === 401 || status === 403) {
          navigate("/login", {
            replace: true,
            state: { reason: "unauthorized" },
          });
          return;
        }

        setError(
          (err as Error).message ||
            t("common.error", { defaultValue: "Error" }),
        );
      } finally {
        setUploading(false);
        if (fileRef.current) fileRef.current.value = "";
      }
    },
    [fetchUser, navigate, t],
  );

  if (!user) return null;

  return (
    <Stack spacing={2} alignItems="center">
      <Box sx={{ position: "relative", display: "inline-block" }}>
        <Avatar
          src={src}
          alt={user.username}
          variant={variant}
          sx={{ width: size, height: size, fontSize: size * 0.35 }}
        >
          {user.username?.charAt(0).toUpperCase()}
        </Avatar>
        {uploading && (
          <Box
            sx={{
              position: "absolute",
              top: 0,
              left: 0,
              width: size,
              height: size,
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              borderRadius:
                variant === "circular" ? "50%" : variant === "rounded" ? 1 : 0,
              bgcolor: "rgba(0, 0, 0, 0.5)",
            }}
          >
            <CircularProgress size={size * 0.4} sx={{ color: "white" }} />
          </Box>
        )}
      </Box>
      <input
        hidden
        ref={fileRef}
        type="file"
        accept="image/png,image/jpeg,image/gif,image/webp,image/bmp,image/tiff,image/x-tga,image/qoi,image/x-portable-bitmap"
        onChange={onChange}
      />
      <Button
        size="small"
        variant="outlined"
        onClick={pick}
        disabled={uploading}
        sx={{ minWidth: 140 }}
      >
        <span
          style={{
            display: "inline-block",
            width: "100%",
            textAlign: "center",
          }}
        >
          {uploading
            ? t("profile.messages.uploading")
            : t("profile.messages.changeAvatar")}
        </span>
      </Button>
      {error && (
        <Typography
          variant="caption"
          color="error"
          sx={{ maxWidth: size + 40, textAlign: "center" }}
        >
          {error}
        </Typography>
      )}
    </Stack>
  );
};

export default AvatarUploader;
