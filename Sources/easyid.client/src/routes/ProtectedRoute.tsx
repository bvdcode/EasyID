import React, { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import UsersService from "../services/usersService";

const ProtectedRoute: React.FC<{ children: React.ReactElement }> = ({
  children,
}) => {
  const [status, setStatus] = useState<"checking" | "allowed" | "denied">(
    "checking",
  );

  useEffect(() => {
    let mounted = true;
    UsersService.me()
      .then(() => mounted && setStatus("allowed"))
      .catch(() => mounted && setStatus("denied"));
    return () => {
      mounted = false;
    };
  }, []);

  if (status === "checking") return null; // can add spinner later
  if (status === "denied") return <Navigate to="/login" replace />;
  return children;
};

export default ProtectedRoute;
