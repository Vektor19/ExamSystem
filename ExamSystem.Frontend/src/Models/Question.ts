import { QuestionOption } from "./QuestionOption";

export interface Question {
    questionId: string;
    questionText: string;
    type: string;
    imageUrl: string;
    maxPoints: number;
    options: QuestionOption[];
    exam: any;
  }
  