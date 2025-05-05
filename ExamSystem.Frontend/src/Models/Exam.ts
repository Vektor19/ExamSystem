import { User } from "./User";

export interface Exam {
  examId: string;
  name: string;
  createdDate: string;
  startDate: string;
  endDate: string;
  joinCode: string;
  status: string;
  createdBy: User;
  questionCount: number;
  participantCount: number;
}
