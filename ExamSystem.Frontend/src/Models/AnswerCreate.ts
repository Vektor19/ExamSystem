export interface AnswerCreate {
  userId: string;
  questionId: string;
  examId: string;
}

export interface OpenAnswerCreate extends AnswerCreate {
  answerText: string;
}

export interface OptionAnswerCreate extends AnswerCreate {
  questionOptionId: string;
}