import examSystemApi from "../Api/examSystemApi";
import { Question } from "../Models/Question";
import { QuestionCreate } from "../Models/QuestionCreate";

class QuestionService {
  async getAllExamById(examId: string): Promise<Question[]> {
    try {
      const res = await examSystemApi.get("/question/by-exam/" + examId);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Questions not found");
    }
  }
  async createQuestion(question: QuestionCreate): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/question/", question);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Question not created");
    }
  }
}

export default new QuestionService();
