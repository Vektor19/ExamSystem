import examSystemApi from "../Api/examSystemApi";
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
  async getAllNotCompletedByUserId(userId: string, examId: string): Promise<Question[]> {
    try {
      const res = await examSystemApi.get("/question/not-completed/by-user/"+userId+"/exam/" + examId);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Questions not found");
    }
  }

  async createQuestion(question: QuestionCreate): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/question/", question);
      return res.data.success;
    } catch (err: any) {
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
}

export default new QuestionService();
