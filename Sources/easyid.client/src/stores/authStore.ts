import { create } from "zustand";
import AuthService, { AuthTokens } from "../services/authService";

const ACCESS_KEY = "easyid.accessToken";
const REFRESH_KEY = "easyid.refreshToken";
const EXPIRES_KEY = "easyid.expiresAt";

let refreshTimer: number | null = null;

function persist(tokens: { accessToken: string | null; refreshToken: string | null; expiresAt?: number | null }) {
  if (tokens.accessToken) localStorage.setItem(ACCESS_KEY, tokens.accessToken);
  else localStorage.removeItem(ACCESS_KEY);
  if (tokens.refreshToken) localStorage.setItem(REFRESH_KEY, tokens.refreshToken);
  else localStorage.removeItem(REFRESH_KEY);
  if (tokens.expiresAt) localStorage.setItem(EXPIRES_KEY, String(tokens.expiresAt));
  else localStorage.removeItem(EXPIRES_KEY);
}

function loadPersisted() {
  const access = localStorage.getItem(ACCESS_KEY);
  const refresh = localStorage.getItem(REFRESH_KEY);
  const expRaw = localStorage.getItem(EXPIRES_KEY);
  const expiresAt = expRaw ? Number(expRaw) : null;
  return { accessToken: access, refreshToken: refresh, expiresAt };
}

export interface AuthState {
  accessToken: string | null;
  refreshToken: string | null;
  expiresAt: number | null;
  isAuthenticated: boolean;
  login: (username: string, password: string) => Promise<void>;
  setTokens: (tokens: AuthTokens) => void;
  refreshTokens: () => Promise<void>;
  scheduleRefresh: () => void;
  logout: () => void;
}

export const authStore = create<AuthState>((set, get) => {
  const persisted = loadPersisted();
  return {
    ...persisted,
    isAuthenticated: Boolean(persisted.accessToken),
  async login(username: string, password: string) {
    const tokens = await AuthService.login(username, password);
    get().setTokens(tokens);
  },
  setTokens(tokens: AuthTokens) {
    if (refreshTimer) {
      window.clearTimeout(refreshTimer);
      refreshTimer = null;
    }
    const expiresAt = tokens.expiresIn ? Date.now() + tokens.expiresIn * 1000 : null;
    const next = {
      accessToken: tokens.accessToken ?? null,
      refreshToken: tokens.refreshToken ?? null,
      expiresAt,
      isAuthenticated: Boolean(tokens.accessToken),
    };
    set(next);
    persist(next);
    get().scheduleRefresh();
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
  scheduleRefresh() {
    if (refreshTimer) {
      window.clearTimeout(refreshTimer);
      refreshTimer = null;
    }
    const { expiresAt } = get();
    if (!expiresAt) return;
    const now = Date.now();
    const leadMs = 30_000; // refresh 30s before expiry
    const delay = Math.max(1_000, expiresAt - now - leadMs);
    refreshTimer = window.setTimeout(() => {
      get().refreshTokens().catch(() => {
        // ignore; route guard will redirect later
      });
    }, delay);
  },
  logout() {
    if (refreshTimer) {
      window.clearTimeout(refreshTimer);
      refreshTimer = null;
    }
    set({ accessToken: null, refreshToken: null, expiresAt: null, isAuthenticated: false });
    persist({ accessToken: null, refreshToken: null, expiresAt: null });
  },
  };
});
