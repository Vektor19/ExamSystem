import examSystemApi from "../Api/examSystemApi";
import { Exam } from "../Models/Exam";
import { ExamCreate } from "../Models/ExamCreate";
import { ExamUpdate } from "../Models/ExamUpdate";

class ExamService {
  async getExamById(id: string): Promise<Exam> {
    try {
      const res = await examSystemApi.get("/exam/" + id);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Exam not found");
    }
  }
  async createExam(exam: ExamCreate): Promise<Exam> {
    try {
      const res = await examSystemApi.post("/exam/", exam);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Exam not created");
    }
  }
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
  async updateExam(id: string, exam: ExamUpdate): Promise<boolean> {
    try {
      const res = await examSystemApi.put("/exam/" + id, exam);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Exam not updated");
    }
  }
}

export default new ExamService();
