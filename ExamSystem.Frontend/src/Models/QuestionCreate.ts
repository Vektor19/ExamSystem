import { QuestionOptionCreate } from "./QuestionOptionCreate";

export interface QuestionCreate {
    examId: string;
    questionText: string;
    type: string;
    imageUrl: string;
    maxPoints: number;
    options: QuestionOptionCreate[];
  }
  