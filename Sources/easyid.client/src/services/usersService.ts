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
}

