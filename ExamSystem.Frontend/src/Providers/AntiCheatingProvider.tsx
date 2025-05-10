import React, {
  createContext,
  useContext,
  useEffect,
  useRef,
  useState,
  ReactNode,
} from "react";
import { useExamSession } from "./ExamSessionProvider";
import ViolationService from "../Services/ViolationService";
import { Violation } from "../Models/Violation";
import { ViolationType } from "../Models/ViolationType";
import { useExams } from "./ExamsProvider";
import { useNotification } from "./NotificationProvider";

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
  const { studentExam, setStudentExam } = useExamSession();
  const { fetchStudentExams } = useExams();
  const [violations, setViolations] = useState<Violation[]>([]);
  const [isViolationsLoading, setIsViolationsLoading] = useState(true);
  const violationsRef = useRef<Violation[]>([]);
  const { showNotification } = useNotification();

  useEffect(() => {
    violationsRef.current = violations;
    console.log("Actual violations:", violations);
  }, [violations]);

  const fetchViolations = async () => {
    setIsViolationsLoading(true);
    if (studentExam) {
      try {
        const violationsData = await ViolationService.getAllByExamUserId(
          studentExam.examUser.examUserId
        );
        setViolations(violationsData);
        console.log("ViolationsFetch:", violationsData);
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
        const isNowCritical = violationsRef.current.length >= 2;

        const violation = await ViolationService.reportViolation({
          examUserId: studentExam.examUser.examUserId,
          violationType: type,
          description,
          isCritical: isNowCritical ? true : isCritical,
        });
        setViolations((prev) => [...prev, violation]);
        console.log("ViolationsAfterRegister:", [
          ...violationsRef.current,
          violation,
        ]);
        showNotification(
          `Violation detected! ${violationsRef.current.length}/2`,
          "error"
        );
      } catch (err) {
        console.error("Error registering violation:", err);
      }
      fetchStudentExams();
    }
  };

  useEffect(() => {
    fetchViolations();
  }, [studentExam]);

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

    const handleCopy = () => {
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
