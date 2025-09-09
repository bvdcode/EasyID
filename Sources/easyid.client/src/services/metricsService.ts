import apiClient from "../api/apiClient";

export default class MetricsService {
  static async hasUsers(): Promise<boolean> {
    const res = await apiClient.get<boolean>("/metrics/has-users", {
      headers: { "x-no-auth": "true" },
    });
    // Some backends return strings "true"/"false"; normalize just in case
    const data: unknown = res.data;
    if (typeof data === "boolean") return data;
    if (typeof data === "string") return data.toLowerCase() === "true";
    return Boolean(data);
  }
}
