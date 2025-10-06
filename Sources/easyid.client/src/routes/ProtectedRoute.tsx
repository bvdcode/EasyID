import React, { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import UsersService from "../services/usersService";
import { userStore } from "../stores/userStore";
import { Box, CircularProgress } from "@mui/material";

const ProtectedRoute: React.FC<{ children: React.ReactElement }> = ({
  children,
}) => {
  const [status, setStatus] = useState<"checking" | "allowed" | "denied">(
    "checking",
  );

  useEffect(() => {
    let mounted = true;
    UsersService.get("me")
      .then((user) => {
        if (!mounted) return;
        userStore.getState().setUser(user);
        setStatus("allowed");
      })
      .catch(() => {
        if (mounted) setStatus("denied");
      });
    return () => {
      mounted = false;
    };
  }, []);

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
  if (status === "denied")
    return <Navigate to="/login" replace state={{ reason: "unauthorized" }} />;
  return children;
};

export default ProtectedRoute;
