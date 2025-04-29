import Home from "./Home/Home.tsx";
import { Navigate, Route, Routes } from "react-router-dom";
import PublicLayout from "./Layouts/PublicLayout.tsx";
import PrivateRoute from "./Routes/PrivateRoute.tsx";
import DashboardLayout from "./Layouts/DashboardLayout.tsx";
import Dashboard from "./Dashboard/Dashboard.tsx";
import { AuthProvider } from "../Providers/AuthProvider.tsx";
import RegisterPage from "./AuthPages/RegisterPage.tsx";
import LoginPage from "./AuthPages/LoginPage.tsx";
import AuthRoute from "./Routes/AuthRoute.tsx";
import LogoutRoute from "./Routes/LogoutRoute.tsx";

const App = () => {
  return (
    <>
      <AuthProvider>
        <Routes>
          <Route element={<PublicLayout />}>
            <Route path="/" element={<Home />} />
            <Route
              path="/login"
              element={
                <AuthRoute>
                  <LoginPage />
                </AuthRoute>
              }
            />
            <Route
              path="/register"
              element={
                <AuthRoute>
                  <RegisterPage />
                </AuthRoute>
              }
            />
          </Route>
          <Route
            path="/dashboard"
            element={
              <PrivateRoute>
                <DashboardLayout />
              </PrivateRoute>
            }
          >
            <Route index element={<Dashboard />} />
          </Route>
          <Route path="*" element={<Navigate to="/" />} />
          <Route path="/logout" element={<LogoutRoute />} />
        </Routes>
      </AuthProvider>
    </>
  );
};

export default App;
