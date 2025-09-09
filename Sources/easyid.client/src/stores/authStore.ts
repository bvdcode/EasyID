import { create } from "zustand";
import AuthService, { AuthTokens } from "../services/authService";

export interface AuthState {
  accessToken: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  login: (username: string, password: string) => Promise<void>;
  setTokens: (tokens: AuthTokens) => void;
  refreshTokens: () => Promise<void>;
  logout: () => void;
}

export const authStore = create<AuthState>((set, get) => ({
  accessToken: null,
  refreshToken: null,
  isAuthenticated: false,
  async login(username: string, password: string) {
    const tokens = await AuthService.login(username, password);
    get().setTokens(tokens);
  },
  setTokens(tokens: AuthTokens) {
    set({
      accessToken: tokens.accessToken ?? null,
      refreshToken: tokens.refreshToken ?? null,
      isAuthenticated: Boolean(tokens.accessToken),
    });
  },
  async refreshTokens() {
    const tokens = await AuthService.refresh();
    get().setTokens(tokens);
  },
  logout() {
    set({ accessToken: null, refreshToken: null, isAuthenticated: false });
  },
}));
