import apiClient from "../api/apiClient";
import { API_BASE_URL } from "../api/config";

export interface UserDto {
  id: string;
  username: string;
  email: string;
  firstName?: string;
  lastName?: string;
  middleName?: string;
}

export interface UpdateUserPayload {
  firstName?: string;
  lastName?: string;
  middleName?: string;
}

export default class UsersService {
  /**
   * Generic fetch by id or "me" (GET users/{id})
   */
  static async get(id: string): Promise<UserDto> {
    const res = await apiClient.get<UserDto>(`/users/${id}`);
    return res.data;
  }

  /**
   * Build public avatar URL. NOTE: server does not accept "me" here (no auth), must use concrete user id.
   */
  static avatarUrl(userId: string): string {
    return `${API_BASE_URL}/users/${userId}/avatar`; // no extension; server handles content type
  }

  /**
   * Lightweight ETag-based avatar fetch (optional usage). Returns a blob URL and ETag.
   * If server doesn't send ETag, it simply refetches each call.
   */
  static async fetchAvatarWithCache(
    userId: string,
    previousEtag?: string,
  ): Promise<{ blobUrl: string; etag?: string; notModified?: boolean }> {
    const headers: Record<string, string> = {};
    if (previousEtag) headers["If-None-Match"] = previousEtag;
    const res = await fetch(this.avatarUrl(userId), { headers });
    if (res.status === 304) {
      return { blobUrl: "", etag: previousEtag, notModified: true };
    }
    if (!res.ok) throw new Error(`Avatar fetch failed: ${res.status}`);
    const etag = res.headers.get("ETag") || undefined;
    const blob = await res.blob();
    const blobUrl = URL.createObjectURL(blob);
    return { blobUrl, etag };
  }

  /**
   * Upload avatar for current (or specific) user. (PUT users/{id}/avatar)
   * @param file image file
   * @param userId defaults to "me" for authenticated user
   */
  static async updateAvatar(file: File, userId: string = "me"): Promise<void> {
    const form = new FormData();
    form.append("file", file);
    await apiClient.put(`/users/${userId}/avatar`, form, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  }

  /**
   * Change password (POST users/{id}/password)
   */
  static async changePassword(
    oldPassword: string,
    newPassword: string,
    userId: string = "me",
  ): Promise<void> {
    await apiClient.post(`/users/${userId}/password`, {
      oldPassword,
      newPassword,
    });
  }

  /**
   * Patch personal info (PATCH users/{id})
   */
  static async updatePersonalInfo(
    data: UpdateUserPayload,
    userId: string = "me",
  ): Promise<void> {
    await apiClient.patch(`/users/${userId}`, data);
  }
}
