import React, { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import UsersService from "../services/usersService";
import { userStore } from "../stores/userStore";

const ProtectedRoute: React.FC<{ children: React.ReactElement }> = ({
  children,
}) => {
  const [status, setStatus] = useState<"checking" | "allowed" | "denied">(
    "checking",
  );

  useEffect(() => {
    let mounted = true;
    UsersService.me()
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

  if (status === "checking") return null; // can add spinner later
  if (status === "denied") return <Navigate to="/login" replace />;
  return children;
};

export default ProtectedRoute;
