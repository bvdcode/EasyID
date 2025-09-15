import apiClient from "../api/apiClient";

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

  static avatarUrl(userId: string, format: "webp" | "png" = "webp"): string {
    return `/api/v1/users/${userId}/avatar.${format}`;
  }

  static async updateAvatar(file: File): Promise<void> {
    const form = new FormData();
    form.append("file", file);
    await apiClient.put(`/users/me/avatar`, form, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  }
}
