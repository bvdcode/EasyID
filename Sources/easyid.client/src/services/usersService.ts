import apiClient from "../api/apiClient";
import { API_BASE_URL } from "../api/config";

export interface UserDto {
  id: string;
  username: string;
  email: string;
}

export default class UsersService {
  static async me(): Promise<UserDto> {
    const res = await apiClient.get<UserDto>("/users/me");
    return res.data;
  }

  static avatarUrl(userId: string): string {
    return `${API_BASE_URL}/users/${userId}/avatar.webp`;
  }

  static async updateAvatar(file: File): Promise<void> {
    const form = new FormData();
    form.append("file", file);
    await apiClient.put(`/users/me/avatar`, form, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  }

  static async changePassword(oldPassword: string, newPassword: string): Promise<void> {
    await apiClient.post(`/users/me/password`, {
      oldPassword,
      newPassword,
    });
  }

  static async updatePersonalInfo(data: { firstName?: string; lastName?: string; middleName?: string }): Promise<void> {
    // Assuming a conventional endpoint; adjust to your actual API if different
    await apiClient.put(`/users/me`, data);
  }
}
