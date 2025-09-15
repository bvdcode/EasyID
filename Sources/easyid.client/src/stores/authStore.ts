import { create } from "zustand";
import AuthService, { AuthTokens } from "../services/authService";

const ACCESS_KEY = "easyid.accessToken";
const REFRESH_KEY = "easyid.refreshToken";

function persist(tokens: { accessToken: string | null; refreshToken: string | null }) {
  if (tokens.accessToken) localStorage.setItem(ACCESS_KEY, tokens.accessToken);
  else localStorage.removeItem(ACCESS_KEY);
  if (tokens.refreshToken) localStorage.setItem(REFRESH_KEY, tokens.refreshToken);
  else localStorage.removeItem(REFRESH_KEY);
}

function loadPersisted() {
  const access = localStorage.getItem(ACCESS_KEY);
  const refresh = localStorage.getItem(REFRESH_KEY);
  return { accessToken: access, refreshToken: refresh };
}

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
  ...loadPersisted(),
  isAuthenticated: Boolean(loadPersisted().accessToken),
  async login(username: string, password: string) {
    const tokens = await AuthService.login(username, password);
    get().setTokens(tokens);
  },
  setTokens(tokens: AuthTokens) {
    const next = {
      accessToken: tokens.accessToken ?? null,
      refreshToken: tokens.refreshToken ?? null,
      isAuthenticated: Boolean(tokens.accessToken),
    };
    set(next);
    persist(next);
  },
  async refreshTokens() {
    const currentRefresh = get().refreshToken;
    if (!currentRefresh) {
      get().logout();
      throw new Error("No refresh token available");
    }
    const tokens = await AuthService.refresh(currentRefresh);
    get().setTokens(tokens);
  },
  logout() {
    set({ accessToken: null, refreshToken: null, isAuthenticated: false });
    persist({ accessToken: null, refreshToken: null });
  },
}));
