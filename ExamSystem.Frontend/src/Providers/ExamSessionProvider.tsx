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
import { useParams } from "react-router";
import { useExams } from "./ExamsProvider";

type ExamSessionContextType = {
  studentExam: StudentExam | null;
  notCompletedQuestions: Question[] | null;
  setStudentExam: (exam: StudentExam) => void;
  fetchNotCompletedQuestions: () => Promise<void>;
  isQuestionsLoading: boolean;
  makeOpenAnswer: (questionId: string, answerText: string) => Promise<void>;
  makeOptionAnswer: (
    questionId: string,
    questionOptionId: string
  ) => Promise<void>;
};

const ExamSessionContext = createContext<ExamSessionContextType | undefined>(
  undefined
);

export const ExamSessionProvider = ({ children }: { children: ReactNode }) => {
  const { id } = useParams<{ id: string }>();
  const { studentExams, fetchStudentExams } = useExams();
  const [studentExam, setStudentExam] = useState<StudentExam | null>(null);
  const [notCompletedQuestions, setNotCompletedQuestions] = useState<
    Question[] | null
  >(null);
  const [isQuestionsLoading, setIsQuestionsLoading] = useState(true);

  useEffect(() => {
    const loadExam = async () => {
      let exam = studentExams?.find((e) => e.examId === id);
      if (!exam) {
        await fetchStudentExams();
        exam = studentExams?.find((e) => e.examId === id);
      }
      if (exam) setStudentExam(exam);
    };
    loadExam();
  }, [id, studentExams]);

  useEffect(() => {
    if (studentExam) {
      fetchNotCompletedQuestions();
    }
  }, [studentExam]);

  const fetchNotCompletedQuestions = async () => {
    setIsQuestionsLoading(true);
    try {
      const token = localStorage.getItem("token");
      const userId = token ? TokenParser.parseIdFromToken(token) : null;
      if (!userId || !studentExam) {
        setNotCompletedQuestions(null);
        return;
      }
      const questions = await QuestionService.getAllNotCompletedByUserId(
        userId,
        studentExam.examId
      );
      setNotCompletedQuestions(questions);
    } catch (err) {
      console.error(err);
      setNotCompletedQuestions(null);
    } finally {
      setIsQuestionsLoading(false);
    }
  };

  const makeOpenAnswer = async (questionId: string, answerText: string) => {
    try {
      const token = localStorage.getItem("token");
      const userId = token ? TokenParser.parseIdFromToken(token) : null;
      if (!userId || !studentExam) return;
      await AnswerService.createOpenAnswer({
        userId,
        questionId,
        examId: studentExam.examId,
        answerText,
      });
      fetchNotCompletedQuestions();
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
      const userId = token ? TokenParser.parseIdFromToken(token) : null;
      if (!userId || !studentExam) return;
      await AnswerService.createOptionAnswer({
        userId,
        questionId,
        examId: studentExam.examId,
        questionOptionId,
      });
      fetchNotCompletedQuestions();
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
