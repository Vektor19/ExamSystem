import examSystemApi from "../Api/examSystemApi";
import { OpenAnswerCreate, OptionAnswerCreate } from "../Models/AnswerCreate";

class AnswerService {
  async createOpenAnswer(answer: OpenAnswerCreate): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/answer/open-type", answer);
      return res.data.success;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Answer not created");
    }
  }
  
  async createOptionAnswer(answer: OptionAnswerCreate): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/answer/option-type", answer);
      return res.data.success;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Answer not created");
    }
  }
}

export default new AnswerService();
