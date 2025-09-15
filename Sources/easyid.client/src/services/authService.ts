import apiClient from "../api/apiClient";

export interface AuthTokens {
  accessToken: string;
  refreshToken?: string;
  expiresIn?: number; // seconds
  // Optionally backend may include user snapshot
  user?: {
    id: string;
    username: string;
    email: string;
  };
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

  static async refresh(refreshToken?: string): Promise<AuthTokens> {
    const res = await apiClient.post<AuthTokens>(
      "/auth/refresh",
      { refreshToken },
      { headers: { "x-no-auth": "true" } },
    );
    return res.data;
  }
}
