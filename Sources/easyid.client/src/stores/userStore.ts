import { create } from "zustand";
import UsersService, { UserDto } from "../services/usersService";

interface UserState {
  user: UserDto | null;
  loading: boolean;
  error: string | null;
  fetchUser: () => Promise<UserDto | null>;
  setUser: (user: UserDto | null) => void;
  clear: () => void;
}

export const userStore = create<UserState>((set) => ({
  user: null,
  loading: false,
  error: null,
  setUser: (user) => set({ user }),
  clear: () => set({ user: null, error: null }),
  async fetchUser() {
    set({ loading: true, error: null });
    try {
      const u = await UsersService.me();
      set({ user: u, loading: false });
      return u;
    } catch (e) {
      set({ error: (e as Error).message, loading: false });
      return null;
    }
  },
}));
