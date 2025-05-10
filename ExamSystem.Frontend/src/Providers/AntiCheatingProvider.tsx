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
        const isNowCritical = violationsRef.current.length >= 2 || isCritical;

        const violation = await ViolationService.reportViolation({
          examUserId: studentExam.examUser.examUserId,
          violationType: type,
          description,
          isCritical: isNowCritical,
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
  let violationTimeout: ReturnType<typeof setTimeout> | null = null;
  let isWindowBlurred = false;

  const startViolationTimer = () => {
    if (violationTimeout || isWindowBlurred) return;

    isWindowBlurred = true;

    console.log("User is away from the exam window");
    showNotification("You are away from the exam window GO BACK!!!", "error");

    violationTimeout = setTimeout(() => {
      registerViolation(
        ViolationType.NewPageOpen,
        "User was away from the exam window for more than 5 seconds",
        true
      );
      console.log("User was away from the exam window for more than 5 seconds");
      violationTimeout = null;
      isWindowBlurred = false;
    }, 5000); // 5 секунд
  };

  const cancelViolationTimer = () => {
    if (violationTimeout) {
      clearTimeout(violationTimeout);
      violationTimeout = null;
    }
    isWindowBlurred = false;
  };

  const handleVisibilityChange = () => {
    if (document.hidden) {
      startViolationTimer();
    }
  };

  const handleWindowBlur = () => {
    startViolationTimer();
  };

  const handleWindowFocus = () => {
    cancelViolationTimer();
    registerViolation(
        ViolationType.NewPageOpen,
        "User lost focus",
        false
      );
    console.log("User returned to window in time");
  };

  const handleCopy = () => {
    registerViolation(ViolationType.CopyPaste, "User copied text", false);
  };

  const handleKeyDown = (e: KeyboardEvent) => {
    if (e.key === "F12") {
      console.log("F12 pressed");
    }
    if (e.altKey && e.key === "Tab") {
      e.preventDefault();
      console.log("Alt + Tab pressed");
    }
    if (e.ctrlKey && (e.key === "T" || e.key === "t")) {
      e.preventDefault();
      console.log("Ctrl + T pressed");
    }
    if (e.key === "PrintScreen") {
      console.log("PrintScreen pressed");
    }
    if (e.ctrlKey && (e.key === "S" || e.key === "s")) {
      console.log("Ctrl + S pressed");
    }
  };

  window.addEventListener("visibilitychange", handleVisibilityChange);
  window.addEventListener("blur", handleWindowBlur);
  window.addEventListener("focus", handleWindowFocus);
  window.addEventListener("copy", handleCopy);
  document.addEventListener("keydown", handleKeyDown);

  return () => {
    window.removeEventListener("visibilitychange", handleVisibilityChange);
    window.removeEventListener("blur", handleWindowBlur);
    window.removeEventListener("focus", handleWindowFocus);
    window.removeEventListener("copy", handleCopy);
    document.removeEventListener("keydown", handleKeyDown);
    if (violationTimeout) clearTimeout(violationTimeout);
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
