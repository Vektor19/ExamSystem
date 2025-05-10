import React, {
  createContext,
  useContext,
  useEffect,
  useState,
  ReactNode,
} from "react";
import { useExamSession } from "./ExamSessionProvider";
import ViolationService from "../Services/ViolationService";
import { Violation } from "../Models/Violation";
import { ViolationType } from "../Models/ViolationType";

type AntiCheatingContextType = {
  violations: Violation[];
  isViolationsLoading: boolean;
  fetchViolations: () => Promise<void>;
  registerViolation: (
    type: string,
    description: string,
    isCritical: boolean
  ) => Promise<void>;
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
    setIsViolationsLoading(true);
    if (studentExam) {
      try {
        const violationsData = await ViolationService.getAllByExamUserId(
          studentExam.examUser.examUserId
        );
        setViolations(violationsData);
      } catch (err) {
        console.error("Error fetching violations:", err);
        setViolations([]);
      }
    }
    setIsViolationsLoading(false);
  };

  const registerViolation = async (
    type: string,
    description: string,
    isCritical: boolean
  ) => {
    if (studentExam) {
      try {
        const violation = await ViolationService.reportViolation({
          examUserId: studentExam.examUser.examUserId,
          violationType: type,
          description,
          isCritical: violations.length >= 2 ? true : isCritical,
        });

        setViolations((prev) => [...prev, violation]);
      } catch (err) {
        console.error("Error registering violation:", err);
      }
    }
  };

  useEffect(() => {
    const handlePageOpenEvent = async () => {
      if (document.hidden) {
        await registerViolation(
          ViolationType.NewPageOpen,
          "User opened new page",
          false
        );
      }
    };

    const handleCopy = (e: ClipboardEvent) => {
      registerViolation(ViolationType.CopyPaste, "User copied text", false);
    };

    window.addEventListener("visibilitychange", handlePageOpenEvent);
    window.addEventListener("copy", handleCopy);

    return () => {
      window.removeEventListener("visibilitychange", handlePageOpenEvent);
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
