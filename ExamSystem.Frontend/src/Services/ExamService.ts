import examSystemApi from "../Api/examSystemApi";
import { NoValidRequestError } from "../Common/Exceptions/NoValidRequestError";
import { ExamCreate } from "../Models/ExamCreate";
import { ExaminatorExam } from "../Models/ExaminatorExam";
import { ExamUpdate } from "../Models/ExamUpdate";
import { StudentExam } from "../Models/StudentExam";

class ExamService {
  async getExamById(id: string): Promise<ExaminatorExam> {
    try {
      const res = await examSystemApi.get("/exam/" + id);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Exam not found");
    }
  }
  async createExam(exam: ExamCreate): Promise<ExaminatorExam> {
    try {
      const res = await examSystemApi.post("/exam/", exam);
      return res.data;
    } catch (err: any) {
      if (err.response?.status === 400 && err.response?.data?.errors) {
        throw new NoValidRequestError(JSON.stringify(err.response.data.errors));
      }
      throw new Error(err?.response?.data?.message || "Exam not created");
    }
  }
  async getExamsByParticipantId(id: string): Promise<StudentExam[]> {
    try {
      const res = await examSystemApi.get("/exam/by-participant/" + id);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Exams not found");
    }
  }
  async getExamsByCreatedUserId(id: string): Promise<ExaminatorExam[]> {
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
      if (err.response?.status === 400 && err.response?.data?.errors) {
        throw new NoValidRequestError(JSON.stringify(err.response.data.errors));
      }
      throw new Error(err?.response?.data?.message || "Exam not updated");
    }
  }
  async addParticipantToExam(
    id: string,
    participantId: string
  ): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/exam/" + id + "/participants", {
        participantId,
      });
      return res.data.success;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Participant not added");
    }
  }
  async addParticipantToExamByEmail(
    id: string,
    email: string
  ): Promise<boolean> {
    try {
      const res = await examSystemApi.post(
        "/exam/" + id + "/participants/by-email",
        {
          email,
        }
      );
      return res.data.success;
    } catch (err: any) {
      if (err.response?.status === 400 && err.response?.data?.errors) {
        throw new NoValidRequestError(JSON.stringify(err.response.data.errors));
      }
      throw new Error(err?.response?.data?.message || "Participant not added");
    }
  }

  async joinExam(userId: string, joinCode: string): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/exam/join", {
        userId,
        joinCode,
      });
      return res.data.success;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Not joined to exam");
    }
  }
}

export default new ExamService();
