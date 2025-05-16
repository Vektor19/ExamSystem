import examSystemApi from "../Api/examSystemApi";
import { NoValidRequestError } from "../Common/Exceptions/NoValidRequestError";
import { GradeOpenAnswer } from "../Models/GradeOpenAnswer";
import { Question } from "../Models/Question";
import { QuestionCreate } from "../Models/QuestionCreate";

class QuestionService {
  async getAllByExamId(examId: string): Promise<Question[]> {
    try {
      const res = await examSystemApi.get("/question/by-exam/" + examId);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Questions not found");
    }
  }
  async getAllNotCompletedByUserId(
    userId: string,
    examId: string
  ): Promise<Question[]> {
    try {
      const res = await examSystemApi.get(
        "/question/not-completed/by-user/" + userId + "/exam/" + examId
      );
      return res.data;
    } catch (err: any) {
      if (err?.response?.status === 404) {
        return [];
      }
      throw new Error(err?.response?.data?.message || "Questions not found");
    }
  }

  async createQuestion(question: QuestionCreate): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/question/", question);
      return res.data.success;
    } catch (err: any) {
      if (err.response?.status === 400 && err.response?.data?.errors) {
        throw new NoValidRequestError(JSON.stringify(err.response.data.errors));
      }
      throw new Error(err?.response?.data?.message || "Question not created");
    }
  }
  async deleteQuestion(id: string): Promise<boolean> {
    try {
      const res = await examSystemApi.delete("/question/" + id);
      return res.data.success;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Question not deleted");
    }
  }
  async gradeTextQuestion(
    questionId: string,
    gradeOpenAnswer: GradeOpenAnswer
  ): Promise<boolean> {
    try {
      const res = await examSystemApi.post(
        "/question/" + questionId + "/grade-openanswer",
        gradeOpenAnswer
      );
      return res.data.success;
    } catch (err: any) {
      if (err.response?.status === 400 && err.response?.data?.errors) {
        throw new NoValidRequestError(JSON.stringify(err.response.data.errors));
      }
      throw new Error(err?.response?.data?.message || "Question not graded");
    }
  }
}

export default new QuestionService();
