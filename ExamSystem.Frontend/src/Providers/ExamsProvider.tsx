import {
  createContext,
  useContext,
  useEffect,
  useState,
  ReactNode,
} from "react";
import TokenParser from "../Services/TokenParser";
import ExamService from "../Services/ExamService";
import { Question } from "../Models/Question";
import QuestionService from "../Services/QuestionService";
import { StudentExam } from "../Models/StudentExam";
import { ExaminatorExam } from "../Models/ExaminatorExam";
import TimeUtils from "../Utils/TimeUtils";

type StudentCheckedExamNotification = {
  examId: string;
  addedDate: string; // ISO format
};
type PinnedExaminatorExam = {
  examId: string;
};

type ExamsContextType = {
  studentExams: StudentExam[] | null;
  examinatorExams: ExaminatorExam[] | null;
  questions: Question[] | null;
  fetchStudentExams: () => Promise<void>;
  fetchExaminatorExams: () => Promise<void>;
  fetchQuestions: (examId: string) => Promise<void>;
  isStudentExamsLoading: boolean;
  isExaminatorExamsLoading: boolean;
  isQuestionsLoading: boolean;
  studentCheckedExams: StudentCheckedExamNotification[];
  pinnedExaminatorExams: PinnedExaminatorExam[] | null;
  removeStudentCheckedExam: (examId: string) => void;
  addPinnedExaminatorExam: (examId: string) => void;
  removePinnedExaminatorExam: (examId: string) => void;
};

const ExamsContext = createContext<ExamsContextType | undefined>(undefined);

const LOCAL_STORAGE_KEYS = {
  studentExams: (userId: string) => `studentExams-${userId}`,
  studentCheckedExams: (userId: string) => `studentCheckedExams-${userId}`,
  pinnedExaminatorExams: (userId: string) => `pinnedExaminatorExams-${userId}`,
};

export const ExamsProvider = ({ children }: { children: ReactNode }) => {
  const [studentExams, setStudentExams] = useState<StudentExam[] | null>(null);
  const [examinatorExams, setExaminatorExams] = useState<ExaminatorExam[] | null>(null);
  const [questions, setQuestions] = useState<Question[] | null>(null);
  const [isStudentExamsLoading, setIsStudentExamsLoading] = useState(true);
  const [isExaminatorExamsLoading, setIsExaminatorExamsLoading] = useState(true);
  const [isQuestionsLoading, setIsQuestionsLoading] = useState(true);
  const [studentCheckedExams, setStudentCheckedExamsState] = useState<StudentCheckedExamNotification[]>([]);
  const [pinnedExaminatorExams, setPinnedExaminatorExams] = useState<PinnedExaminatorExam[]>([]);

  const getUserId = () => {
    const token = localStorage.getItem("token");
    return token ? TokenParser.parseIdFromToken(token) : null;
  };

  const loadStudentCheckedExams = (userId: string) => {
    const raw = localStorage.getItem(LOCAL_STORAGE_KEYS.studentCheckedExams(userId));
    const data: StudentCheckedExamNotification[] = raw ? JSON.parse(raw) : [];

    // Check if the exams are older than 2 days and remove them
    const now = new Date();
    const filtered = data.filter(n => {
      const added = new Date(n.addedDate);
      const diffDays = (now.getTime() - added.getTime()) / (1000 * 60 * 60 * 24);
      return diffDays <= 2;
    });
    localStorage.setItem(LOCAL_STORAGE_KEYS.studentCheckedExams(userId), JSON.stringify(filtered));
    setStudentCheckedExamsState(filtered);
    return filtered;
  };
  const loadPinnedExaminatorExams = (userId: string) => {
    const raw = localStorage.getItem(LOCAL_STORAGE_KEYS.pinnedExaminatorExams(userId));
    const data: PinnedExaminatorExam[] = raw ? JSON.parse(raw) : [];
    return data;
  }
  const savePinnedExaminatorExams = (userId: string, list: PinnedExaminatorExam[]) => {
    localStorage.setItem(LOCAL_STORAGE_KEYS.pinnedExaminatorExams(userId), JSON.stringify(list));
    setPinnedExaminatorExams(list);
  };
  const addPinnedExaminatorExam = (examId: string) => {
    const userId = getUserId();
    if (!userId) return;
    const newPinnedExam = { examId };
    const currentPinnedExams = loadPinnedExaminatorExams(userId);
    const updatedPinnedExams = [...currentPinnedExams, newPinnedExam];
    savePinnedExaminatorExams(userId, updatedPinnedExams);
  };
  const removePinnedExaminatorExam = (examId: string) => {
    const userId = getUserId();
    if (!userId) return;
    const filtered = pinnedExaminatorExams.filter(n => n.examId !== examId);
    savePinnedExaminatorExams(userId, filtered);
  };

  const saveStudentCheckedExams = (userId: string, list: StudentCheckedExamNotification[]) => {
    localStorage.setItem(LOCAL_STORAGE_KEYS.studentCheckedExams(userId), JSON.stringify(list));
    setStudentCheckedExamsState(list);
  };

  const removeStudentCheckedExam = (examId: string) => {
    const userId = getUserId();
    if (!userId) return;
    const filtered = studentCheckedExams.filter(n => n.examId !== examId);
    saveStudentCheckedExams(userId, filtered);
  };

  const fetchStudentExams = async () => {
    setIsStudentExamsLoading(true);
    try {
      const token = localStorage.getItem("token");
      if (!token) return setStudentExams(null);

      const userId = TokenParser.parseIdFromToken(token);
      if (!userId) return setStudentExams(null);

      const oldExamsRaw = localStorage.getItem(LOCAL_STORAGE_KEYS.studentExams(userId));
      const oldExams: StudentExam[] = oldExamsRaw ? JSON.parse(oldExamsRaw) : [];

      const examData = await ExamService.getExamsByParticipantId(userId);
      examData.forEach((exam) => {
        exam.startDate = TimeUtils.formatUtcToLocalIso(exam.startDate);
        exam.endDate = TimeUtils.formatUtcToLocalIso(exam.endDate);
      });

      const changedExams = examData.filter(newExam => {
        const old = oldExams.find(e => e.examId === newExam.examId);
        return old && !old.examUser.isChecked && newExam.examUser.isChecked;
      });

      const now = new Date().toISOString();
      const currentNotifications = loadStudentCheckedExams(userId);

      const updatedNotifications = [
        ...currentNotifications,
        ...changedExams
          .filter(e => !currentNotifications.some(n => n.examId === e.examId))
          .map(e => ({ examId: e.examId, addedDate: now })),
      ];

      saveStudentCheckedExams(userId, updatedNotifications);
      localStorage.setItem(LOCAL_STORAGE_KEYS.studentExams(userId), JSON.stringify(examData));
      setStudentExams(examData);
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
      if (!token) return setExaminatorExams(null);

      const userId = TokenParser.parseIdFromToken(token);
      if (!userId) return setExaminatorExams(null);

      const examData = await ExamService.getExamsByCreatedUserId(userId);
      examData.forEach((exam) => {
        exam.startDate = TimeUtils.formatUtcToLocalIso(exam.startDate);
        exam.endDate = TimeUtils.formatUtcToLocalIso(exam.endDate);
      });

      setExaminatorExams(examData);
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

  useEffect(() => {
    const userId = getUserId();
    if (userId) {
      loadStudentCheckedExams(userId); // ініціалізація
    }
  }, []);

  return (
    <ExamsContext.Provider
      value={{
        studentExams,
        examinatorExams,
        questions,
        fetchStudentExams,
        fetchExaminatorExams,
        fetchQuestions,
        isStudentExamsLoading,
        isExaminatorExamsLoading,
        isQuestionsLoading,
        studentCheckedExams,
        pinnedExaminatorExams,
        removeStudentCheckedExam,
        addPinnedExaminatorExam,
        removePinnedExaminatorExam,
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
