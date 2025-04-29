import {
  createContext,
  useContext,
  useEffect,
  useState,
  ReactNode,
} from "react";
import AuthService from "../Services/AuthService";

type AuthContextType = {
  isAuthenticated: boolean;
  loading: boolean;
  login: (
    email: string,
    password: string
  ) => Promise<{ success: boolean; message: string }>;
  register: (
    email: string,
    password: string
  ) => Promise<{ success: boolean; message: string }>;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const checkToken = async () => {
      const token = localStorage.getItem("token");
      if (token) {
        const valid: boolean = await AuthService.validateToken(token);
        setIsAuthenticated(valid);
      }
      setLoading(false);
    };
    checkToken();
  }, []);

  const login = async (email: string, password: string) => {
    try {
      const token = await AuthService.login(email, password);
      localStorage.setItem("token", token);
      setIsAuthenticated(true);
      return {
        success: true,
        message: "Login successful",
      };
    } catch (err: any) {
      setIsAuthenticated(false);
      return {
        success: false,
        message: err?.response?.data?.message || "Login failed",
      };
    }
  };

  const register = async (email: string, password: string) => {
    try {
      const token = await AuthService.register(email, password);
      localStorage.setItem("token", token);
      setIsAuthenticated(true);
      return {
        success: true,
        message: "Register successful",
      };
    } catch (err: any) {
      return {
        success: false,
        message: err?.response?.data?.message || "Register failed",
      };
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider
      value={{ isAuthenticated, loading, login, register, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
