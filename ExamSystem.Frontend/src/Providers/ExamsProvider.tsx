import {
  createContext,
  useContext,
  useEffect,
  useState,
  ReactNode,
} from "react";
import TokenParser from "../Services/TokenParser";
import { Exam } from "../Models/Exam";
import ExamService from "../Services/ExamService";

type ExamsContextType = {
  studentExams: Exam[] | null;
  examinatorExams: Exam[] | null;
  fetchStudentExams: () => Promise<void>;
  fetchExaminatorExams: () => Promise<void>;
  isStudentExamsLoading: boolean;
  isExaminatorExamsLoading: boolean;
};

const ExamsContext = createContext<ExamsContextType | undefined>(undefined);

export const ExamsProvider = ({ children }: { children: ReactNode }) => {
  const [studentExams, setStudentExams] = useState<Exam[] | null>(null);
  const [examinatorExams, setExaminatorExams] = useState<Exam[] | null>(null);
  const [isStudentExamsLoading, setIsStudentExamsLoading] = useState(true);
  const [isExaminatorExamsLoading, setIsExaminatorExamsLoading] =
    useState(true);

  useEffect(() => {
    fetchStudentExams();
    fetchExaminatorExams();
  }, []);

  const fetchStudentExams = async () => {
    setIsStudentExamsLoading(true);
    try {
      const token = localStorage.getItem("token");
      if (token) {
        const userId = TokenParser.parseIdFromToken(token);
        if (!userId) {
          setStudentExams(null);
          setIsStudentExamsLoading(false);
          return;
        }
        const examData = await ExamService.getExamsByParticipantId(userId);
        setStudentExams(examData);
      } else {
        setStudentExams(null);
      }
    } catch (err) {
      setStudentExams(null);
      console.error(err);
    } finally {
      setIsStudentExamsLoading(false);
    }
  };

  const fetchExaminatorExams = async () => {
    setIsExaminatorExamsLoading(true);
    try {
      const token = localStorage.getItem("token");
      if (token) {
        const userId = TokenParser.parseIdFromToken(token);
        if (!userId) {
          setExaminatorExams(null);
          setIsExaminatorExamsLoading(false);
          return;
        }
        const examData = await ExamService.getExamsByCreatedUserId(userId);
        setExaminatorExams(examData);
      } else {
        setExaminatorExams(null);
      }
    } catch (err) {
      setExaminatorExams(null);
      console.error(err);
    } finally {
      setIsExaminatorExamsLoading(false);
    }
  };

  return (
    <ExamsContext.Provider
      value={{
        studentExams,
        examinatorExams,
        fetchStudentExams,
        fetchExaminatorExams,
        isStudentExamsLoading,
        isExaminatorExamsLoading,
      }}
    >
      {children}
    </ExamsContext.Provider>
  );
};

export const useExams = () => {
  const context = useContext(ExamsContext);
  if (!context) {
    throw new Error("useExams must be used within an ExamsProvider");
  }
  return context;
};
