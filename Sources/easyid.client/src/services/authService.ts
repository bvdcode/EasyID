import apiClient from "../api/apiClient";

export interface AuthTokens {
  accessToken: string;
  refreshToken?: string;
  expiresIn?: number; // seconds
}

export default class AuthService {
  static async login(username: string, password: string): Promise<AuthTokens> {
    const res = await apiClient.post<AuthTokens>(
      "/auth/login",
      {
        username,
        password,
      },
      {
        headers: { "x-no-auth": "true" },
      },
    );
    return res.data;
  }

  static async refresh(): Promise<AuthTokens> {
    const res = await apiClient.post<AuthTokens>("/auth/refresh");
    return res.data;
  }
}
