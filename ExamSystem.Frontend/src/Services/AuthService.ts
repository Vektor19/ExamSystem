import examSystemApi from "../Api/examSystemApi";

class AuthService {
  async login(email: string, password: string): Promise<string> {
    try {
      const res = await examSystemApi.post("/auth/login", { email, password });
      return res.data.token;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Login failed");
    }
  }

  async register(email: string, password: string): Promise<string> {
    try {
      const res = await examSystemApi.post("/auth/register", {
        email,
        password,
      });
      return res.data.token;
    } catch (err: any) {
      throw new Error(err?.response?.data?.message || "Register failed");
    }
  }

  async validateToken(token: string): Promise<boolean> {
    try {
      const res = await examSystemApi.post("/auth/validate-token/", { token });
      return res.status === 200;
    } catch (err) {
      return false;
    }
  }
}

export default new AuthService();
