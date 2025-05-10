import React, {
  createContext,
  useContext,
  useEffect,
  useState,
  ReactNode,
} from "react";
import { useExamSession } from "./ExamSessionProvider";

type Violation = {
  type: string;
  timestamp: string;
};

type AntiCheatingContextType = {
  violations: Violation[];
  isViolationsLoading: boolean;
  fetchViolations: () => Promise<void>;
  registerViolation: (type: string) => void;
};

const AntiCheatingContext = createContext<AntiCheatingContextType | undefined>(
  undefined
);

export const AntiCheatingProvider = ({ children }: { children: ReactNode }) => {
  const { studentExam } = useExamSession();
  const [violations, setViolations] = useState<Violation[]>([]);
  const [isViolationsLoading, setIsViolationsLoading] = useState(true);

  useEffect(() => {
    fetchViolations();
  }, [studentExam]);

  const fetchViolations = async () => {
    if (studentExam) {
      // const response = await ViolationService.getViolations(studentExam.examId);
      // setViolations(response.data);
    }
    setIsViolationsLoading(false);
  };

  const registerViolation = (type: string) => {
    const violation = {
      type,
      timestamp: new Date().toISOString(),
    };
    setViolations((prev) => [...prev, violation]);

    if (studentExam) {
      // ViolationService.reportViolation({
      //   examId: studentExam.examId,
      //   type,
      //   timestamp: violation.timestamp,
      // });
    }
  };

  useEffect(() => {
    const handleVisibilityChange = () => {
      if (document.hidden) {
        registerViolation("TAB_SWITCH");
      }
    };

    const handleCopy = (e: ClipboardEvent) => {
      registerViolation("COPY_ATTEMPT");
    };

    window.addEventListener("visibilitychange", handleVisibilityChange);
    window.addEventListener("copy", handleCopy);

    return () => {
      window.removeEventListener("visibilitychange", handleVisibilityChange);
      window.removeEventListener("copy", handleCopy);
    };
  }, [studentExam]);

  return (
    <AntiCheatingContext.Provider
      value={{
        violations,
        registerViolation,
        isViolationsLoading,
        fetchViolations,
      }}
    >
      {children}
    </AntiCheatingContext.Provider>
  );
};

export const useAntiCheating = () => {
  const context = useContext(AntiCheatingContext);
  if (!context) {
    throw new Error("useAntiCheating must be used within AntiCheatingProvider");
  }
  return context;
};
