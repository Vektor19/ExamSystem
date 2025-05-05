import examSystemApi from "../Api/examSystemApi";
import { Exam } from "../Models/Exam";

class ExamService {
  async getExamsByParticipantId(id: string): Promise<Exam[]> {
    try {
      const res = await examSystemApi.get("/exam/by-participant/" + id);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Exams not found");
    }
  }
  async getExamsByCreatedUserId(id: string): Promise<Exam[]> {
    try {
      const res = await examSystemApi.get("/exam/by-me/" + id);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Exams not found");
    }
  }
}

export default new ExamService();
