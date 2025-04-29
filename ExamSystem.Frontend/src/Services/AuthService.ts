import examSystemApi from "../Api/examSystemApi";
import { RegisterUserRequest } from "../Models/RegisterUserRequest";

class AuthService {
  async login(email: string, password: string): Promise<string> {
    try {
      const res = await examSystemApi.post("/auth/login", { email, password });
      return res.data.token;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Login failed");
    }
  }

  async register(registerUser :RegisterUserRequest ): Promise<string> {
    try {
      const res = await examSystemApi.post("/auth/register", {
        firstName: registerUser.firstname,
        lastname: registerUser.lastname,
        email: registerUser.email,
        password: registerUser.password,
      });
      return res.data.token;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Register failed");
    }
  }

  async validateToken(token: string): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/auth/validate/", { token });
      return res.status === 200;
    } catch (err) {
      return false;
    }
  }
}

export default new AuthService();
