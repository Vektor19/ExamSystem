import examSystemApi from "../Api/examSystemApi";
import { NoValidRequestError } from "../Common/Exceptions/NoValidRequestError";
import { Violation } from "../Models/Violation";
import { ViolationCreate } from "../Models/ViolationCreate";

class ViolationService {
  async getAllByExamUserId(examUserId: string): Promise<Violation[]> {
    try {
      const res = await examSystemApi.get(
        "/violation/by-examuser/" + examUserId
      );
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Violations not found");
    }
  }

  async reportViolation(violation: ViolationCreate): Promise<Violation> {
    try {
      const res = await examSystemApi.post(
        "/violation/exam-user/" + violation.examUserId,
        violation
      );
      return res.data;
    } catch (err: any) {
      if (err.response?.status === 400 && err.response?.data?.errors) {
        throw new NoValidRequestError(JSON.stringify(err.response.data.errors));
      }
      throw new Error(err?.response?.data?.message || "Violation not created");
    }
  }
}

export default new ViolationService();
