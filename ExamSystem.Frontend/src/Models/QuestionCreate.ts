import { QuestionOptionCreate } from "./QuestionOptionCreate";

export interface QuestionCreate {
    examId: string;
    questionText: string;
    type: string;
    imageUrl: string;
    options: QuestionOptionCreate[];
  }
  