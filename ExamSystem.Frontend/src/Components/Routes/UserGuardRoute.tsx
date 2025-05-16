import React, { useEffect } from "react";
import { Navigate } from "react-router-dom";
import { useUser } from "../../Providers/UserProvider";
import LoadingPage from "../Extra/LoadingPage";

const UserGuardRoute = ({ children }: { children: React.ReactNode }) => {
  const { user, loading, fetchUser } = useUser();
  useEffect(() => {
    if (!user && !loading) {
      fetchUser();
    }
  }, [user, loading]);
  if (loading) return <LoadingPage />;
  return user ? <>{children}</> : <Navigate to="/login" />;
};

export default UserGuardRoute;
