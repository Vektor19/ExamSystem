import {
  createContext,
  useContext,
  useEffect,
  useState,
  ReactNode,
} from "react";
import TokenParser from "../Services/TokenParser";
import { Question } from "../Models/Question";
import QuestionService from "../Services/QuestionService";
import { StudentExam } from "../Models/StudentExam";
import AnswerService from "../Services/AnswerService";

type ExamSessionContextType = {
  studentExam: StudentExam | null;
  notCompletedQuestions: Question[] | null;
  setStudentExam: (exam: StudentExam) => void;
  fetchNotCompletedQuestions: () => Promise<void>;
  isQuestionsLoading: boolean;
  makeOpenAnswer: (questionId: string, answerText: string) => Promise<void>;
  makeOptionAnswer: (
    questionId: string,
    questionOptionId: string,
    answerText: string
  ) => Promise<void>;
};

const ExamSessionContext = createContext<ExamSessionContextType | undefined>(
  undefined
);

export const ExamSessionProvider = ({ children }: { children: ReactNode }) => {
  const [studentExam, setStudentExam] = useState<StudentExam | null>(null);
  const [notCompletedQuestions, setNotCompletedQuestions] = useState<
    Question[] | null
  >(null);
  const [isQuestionsLoading, setIsQuestionsLoading] = useState(true);
  useEffect(() => {
    fetchNotCompletedQuestions();
  }, []);

  const fetchNotCompletedQuestions = async () => {
    setIsQuestionsLoading(true);
    try {
      const token = localStorage.getItem("token");
      if (token) {
        const userId = TokenParser.parseIdFromToken(token);
        if (!userId) {
          setNotCompletedQuestions(null);
          setIsQuestionsLoading(false);
          return;
        }
        const questionsData = await QuestionService.getAllNotCompletedByUserId(
          userId,
          studentExam?.examId || ""
        );
        setNotCompletedQuestions(questionsData);
      } else {
        setNotCompletedQuestions(null);
      }
    } catch (err) {
      setNotCompletedQuestions(null);
      console.error(err);
    } finally {
      setIsQuestionsLoading(false);
    }
  };
  const makeOpenAnswer = async (questionId: string, answerText: string) => {
    try {
      const token = localStorage.getItem("token");
      if (token) {
        const userId = TokenParser.parseIdFromToken(token);
        if (!userId) return;
        await AnswerService.createOpenAnswer({
          userId,
          questionId,
          examId: studentExam?.examId || "",
          answerText,
        });
        fetchNotCompletedQuestions();
      }
    } catch (err) {
      console.error(err);
    }
  };
  const makeOptionAnswer = async (
    questionId: string,
    questionOptionId: string
  ) => {
    try {
      const token = localStorage.getItem("token");
      if (token) {
        const userId = TokenParser.parseIdFromToken(token);
        if (!userId) return;
        await AnswerService.createOptionAnswer({
          userId,
          questionId,
          examId: studentExam?.examId || "",
          questionOptionId,
        });
        fetchNotCompletedQuestions();
      }
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <ExamSessionContext.Provider
      value={{
        studentExam,
        setStudentExam,
        notCompletedQuestions,
        fetchNotCompletedQuestions,
        isQuestionsLoading,
        makeOpenAnswer,
        makeOptionAnswer,
      }}
    >
      {children}
    </ExamSessionContext.Provider>
  );
};

export const useExamSession = () => {
  const context = useContext(ExamSessionContext);
  if (!context) {
    throw new Error(
      "useExamSession must be used within an ExamSessionProvider"
    );
  }
  return context;
};
