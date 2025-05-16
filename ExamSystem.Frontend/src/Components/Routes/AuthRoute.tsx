import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../../Providers/AuthProvider';

const AuthRoute = ({ children }: { children: React.ReactNode }) => {
  const { isAuthenticated } = useAuth();

  return !isAuthenticated ? <>{children}</> : <Navigate to="/dashboard" />;
};

export default AuthRoute;
