import examSystemApi from "../Api/examSystemApi";
import { NoValidRequestError } from "../Common/Exceptions/NoValidRequestError";
import { Answer } from "../Models/Answer";
import { OpenAnswerCreate, OptionAnswerCreate } from "../Models/AnswerCreate";

class AnswerService {
  async createOpenAnswer(answer: OpenAnswerCreate): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/answer/open-type", answer);
      return res.data.success;
    } catch (err: any) {
      if (err.response?.status === 400 && err.response?.data?.errors) {
        throw new NoValidRequestError(JSON.stringify(err.response.data.errors));
      }
      throw new Error(err?.response?.data?.message || "Answer not created");
    }
  }

  async createOptionAnswer(answer: OptionAnswerCreate): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/answer/option-type", answer);
      return res.data.success;
    } catch (err: any) {
      if (err.response?.status === 400 && err.response?.data?.errors) {
        throw new NoValidRequestError(JSON.stringify(err.response.data.errors));
      }
      throw new Error(err?.response?.data?.message || "Answer not created");
    }
  }
  async getAllByExamUserId(examUserId: string): Promise<Answer[]> {
    try {
      const res = await examSystemApi.get("/answer/by-examuser/" + examUserId);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Answers not found");
    }
  }
}

export default new AnswerService();
