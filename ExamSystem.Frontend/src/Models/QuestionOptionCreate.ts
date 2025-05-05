export interface QuestionOptionCreate {
    questionId?: string;
    label: string;
    optionText: string;
    isCorrect: boolean;
  }