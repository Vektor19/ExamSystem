import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../Providers/AuthProvider";

const LogoutRoute = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    logout();
    navigate("/", { replace: true });
  }, []);

  return <></>;
};

export default LogoutRoute;
