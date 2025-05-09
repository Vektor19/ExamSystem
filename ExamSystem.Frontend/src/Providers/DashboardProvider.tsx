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
  const [mode, setModeState] = useState<"student" | "examinator">("student");

  useEffect(() => {
    const savedMode = localStorage.getItem("dashboardMode");
    if (savedMode === "student" || savedMode === "examinator") {
      setModeState(savedMode);
    }
  }, []);

  const setMode = (mode: "student" | "examinator") => {
    localStorage.setItem("dashboardMode", mode);
    setModeState(mode);
  };

  return (
    <DashboardContext.Provider value={{ mode, setMode }}>
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
