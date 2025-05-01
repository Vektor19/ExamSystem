import examSystemApi from "../Api/examSystemApi";
import { UpdateUser } from "../Models/UpdateUser";
import { User } from "../Models/User";

class UserService {
  async getUserById(id: string): Promise<User> {
    try {
      const res = await examSystemApi.get("/user/" + id);
      return res.data;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "User not found");
    }
  }
  async updateUser(id: string, user: UpdateUser): Promise<boolean> {
    try {
      const res = await examSystemApi.put("/user/" + id, user);
      return res.data.success;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "User not found");
    }
  }
}

export default new UserService();
