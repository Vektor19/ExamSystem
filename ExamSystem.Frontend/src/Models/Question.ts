import { Exam } from "./Exam";
import { QuestionOption } from "./QuestionOption";

export interface Question {
    questionId: string;
    questionText: string;
    type: string;
    imageUrl: string;
    options: QuestionOption[];
    exam: Exam;
  }
  