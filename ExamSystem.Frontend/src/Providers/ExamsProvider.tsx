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
import { Question } from "../Models/Question";
import QuestionService from "../Services/QuestionService";

type ExamsContextType = {
  studentExams: Exam[] | null;
  examinatorExams: Exam[] | null;
  questions: Question[] | null;
  fetchStudentExams: () => Promise<void>;
  fetchExaminatorExams: () => Promise<void>;
  fetchQuestions: (examId: string) => Promise<void>;
  isStudentExamsLoading: boolean;
  isExaminatorExamsLoading: boolean;
  isQuestionsLoading: boolean;
};

const ExamsContext = createContext<ExamsContextType | undefined>(undefined);

export const ExamsProvider = ({ children }: { children: ReactNode }) => {
  const [studentExams, setStudentExams] = useState<Exam[] | null>(null);
  const [examinatorExams, setExaminatorExams] = useState<Exam[] | null>(null);
  const [isStudentExamsLoading, setIsStudentExamsLoading] = useState(true);
  const [isExaminatorExamsLoading, setIsExaminatorExamsLoading] =
    useState(true);
  const [questions, setQuestions] = useState<Question[] | null>(null);
  const [isQuestionsLoading, setIsQuestionsLoading] = useState(true);
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

  const fetchQuestions = async (examId: string) => {
    setIsQuestionsLoading(true);
    try {
      const questionData = await QuestionService.getAllByExamId(examId);
      setQuestions(questionData);
    } catch (err) {
      setQuestions(null);
      console.error(err);
    } finally {
      setIsQuestionsLoading(false);
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
        questions,
        fetchQuestions,
        isQuestionsLoading,
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
