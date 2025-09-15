import apiClient from "../api/apiClient";
import { VaultData } from "../types";

export default class VaultApiService {
  static async getVaultData(password: string): Promise<VaultData[]> {
    const res = await apiClient.get<VaultData[]>(
      `/vault/${encodeURIComponent(password)}`,
    );
    if (!Array.isArray(res.data)) {
      throw new Error("Invalid response format");
    }
    return res.data;
  }

  static async saveVaultData(
    password: string,
    data: VaultData[],
  ): Promise<void> {
    await apiClient.post(`/vault/${encodeURIComponent(password)}`, data);
  }
}
