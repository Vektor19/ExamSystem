import {
  createContext,
  useContext,
  useEffect,
  useState,
  ReactNode,
} from "react";

type DashboardContextType = {
  mode: "student" | "examinator";
  setMode: (mode: "student" | "examinator") => void;
};

const DashboardContext = createContext<DashboardContextType | undefined>(undefined);

export const DashboardProvider = ({ children }: { children: ReactNode }) => {
  const [mode, setMode] = useState<"student" | "examinator">("student");

  return (
    <DashboardContext.Provider
      value={{ mode, setMode }}
    >
      {children}
    </DashboardContext.Provider>
  );
};

export const useDashboardContext = () => {
  const context = useContext(DashboardContext);
  if (!context) {
    throw new Error("useAuth must be used within an DashboardProvider");
  }
  return context;
};
