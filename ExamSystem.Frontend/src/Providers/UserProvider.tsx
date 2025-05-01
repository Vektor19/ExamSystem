import {
  createContext,
  useContext,
  useEffect,
  useState,
  ReactNode,
} from "react";
import { User } from "../Models/User";
import UserService from "../Services/UserService";
import TokenParser from "../Services/TokenParser";
import { UpdateUser } from "../Models/UpdateUser";

type UserContextType = {
  user: User | null;
  fetchUser: () => Promise<void>;
  loading: boolean;
  updateUser: (user: UpdateUser) => Promise<boolean>;
};

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchUser();
  }, []);

  const fetchUser = async () => {
    setLoading(true);
    try {
      const token = localStorage.getItem("token");
      if (token) {
        const userId = TokenParser.parseIdFromToken(token);
        if (!userId) {
          setUser(null);
          setLoading(false);
          return;
        }
        const userData = await UserService.getUserById(userId);
        setUser(userData);
      } else {
        setUser(null);
      }
    } catch (err) {
      setUser(null);
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const updateUser = async (updateUser: UpdateUser) => {
    setLoading(true);
    try {
      const userId = user?.userId;
      if (!userId) {
        setLoading(false);
        return false;
      }
      const success = await UserService.updateUser(userId, updateUser);
      if (success) {
        await fetchUser();
      }
      return success;
    } catch (err) {
      console.error(err);
      return false;
    } finally {
      setLoading(false);
    }
  };

  return (
    <UserContext.Provider value={{ user, fetchUser, loading, updateUser }}>
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error("useUser must be used within an UserProvider");
  }
  return context;
};
