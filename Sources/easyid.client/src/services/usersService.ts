import apiClient from "../api/apiClient";

export default class UsersService {
  static async me(): Promise<string> {
    const res = await apiClient.get<string>("/users/me");
    return res.data;
  }
}
