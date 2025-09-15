import {
  Stack,
  Avatar,
  Button,
  Typography,
  CircularProgress,
} from "@mui/material";
import { userStore } from "../stores/userStore";
import UsersService from "../services/usersService";
import React, { useCallback, useRef, useState } from "react";

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
  const setUser = userStore((s) => s.setUser);
  const fetchUser = userStore((s) => s.fetchUser);
  const fileRef = useRef<HTMLInputElement | null>(null);
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [version, setVersion] = useState(0);

  const baseUrl = user ? user.avatarUrl || UsersService.avatarUrl(user.id) : "";
  const src = user
    ? `${baseUrl}${baseUrl.includes("?") ? "&" : "?"}v=${version}`
    : undefined;

  const pick = () => fileRef.current?.click();

  const onChange = useCallback(
    async (e: React.ChangeEvent<HTMLInputElement>) => {
      const file = e.target.files?.[0];
      if (!file) return;
      setError(null);
      setUploading(true);
      try {
        await UsersService.updateAvatar(file);
        // Always refresh user to get updated avatarUrl (e.g., if backend sets a new signed URL)
        const refreshed = await fetchUser();
        if (!refreshed && user && !user.avatarUrl) {
          // fallback optimistic update if refetch failed
          setUser({ ...user, avatarUrl: UsersService.avatarUrl(user.id) });
        }
        setVersion((v) => v + 1); // cache-bust
      } catch (err) {
        setError((err as Error).message || "Upload failed");
      } finally {
        setUploading(false);
        if (fileRef.current) fileRef.current.value = "";
      }
    },
    [user, setUser, fetchUser],
  );

  if (!user) return null;

  return (
    <Stack spacing={2} alignItems="center">
      <Avatar
        src={src}
        alt={user.username}
        variant={variant}
        sx={{ width: size, height: size, fontSize: size * 0.35 }}
      >
        {user.username?.charAt(0).toUpperCase()}
      </Avatar>
      {uploading && (
        <CircularProgress
          sx={{
            position: "absolute",
            width: size,
            height: size,
          }}
        />
      )}
      <input
        hidden
        ref={fileRef}
        type="file"
        accept="image/*"
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
          {uploading ? "Uploading..." : "Change avatar"}
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
