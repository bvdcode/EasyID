import apiClient from "../api/apiClient";

interface Metrics {
  hasUsers: boolean;
  serverTime: string; // ISO 8601 format
}

export default class MetricsService {
  static async getMetrics(): Promise<Metrics | null> {
    const res = await apiClient.get<Metrics>("/metrics", {
      headers: { "x-no-auth": "true" },
    });
    return res.data;
  }
}
