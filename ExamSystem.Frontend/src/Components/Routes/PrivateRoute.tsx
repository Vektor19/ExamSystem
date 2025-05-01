import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../../Providers/AuthProvider';
import LoadingPage from '../Extra/LoadingPage';

const PrivateRoute = ({ children }: { children: React.ReactNode }) => {
  const { isAuthenticated, loading } = useAuth();

  if (loading) return <LoadingPage />;

  return isAuthenticated ? <>{children}</> : <Navigate to="/login" />;
};

export default PrivateRoute;
