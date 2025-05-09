import { Participant } from "./Participant";
import { User } from "./User";

export interface StudentExam {
  examId: string;
  name: string;
  createdDate: string;
  startDate: string;
  endDate: string;
  status: string;
  createdBy: User;
  questionCount: number;
  participantCount: number;
  examUser: Participant;
}
