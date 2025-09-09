import axios, {
  AxiosError,
  AxiosInstance,
  InternalAxiosRequestConfig,
} from "axios";
import { API_BASE_URL } from "./config";
import { authStore } from "../stores/authStore";

// Create a dedicated Axios instance
const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
});

// Attach Authorization header if access token exists
apiClient.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  const token = authStore.getState().accessToken;
  if (token) {
    try {
      const headers = config.headers as unknown as {
        set?: (k: string, v: string) => void;
        [key: string]: unknown;
      };
      if (typeof headers.set === "function") {
        headers.set("Authorization", `Bearer ${token}`);
      } else {
        (headers as Record<string, unknown>)[
          "Authorization"
        ] = `Bearer ${token}`;
      }
    } catch {
      // ignore header mutation errors
    }
  }
  return config;
});

// Handle 401 by attempting token refresh once per request
let isRefreshing = false;
let pendingQueue: Array<{
  resolve: (value: unknown) => void;
  reject: (reason?: unknown) => void;
}> = [];

function processQueue(error: AxiosError | null = null) {
  pendingQueue.forEach((p) => {
    if (error) p.reject(error);
    else p.resolve(null);
  });
  pendingQueue = [];
}

apiClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as
      | (InternalAxiosRequestConfig & {
          _retry?: boolean;
        })
      | undefined;

    // If request doesn't require auth or already retried, just reject
    const requiresAuth = !(
      (
        originalRequest?.headers as unknown as
          | Record<string, string>
          | undefined
      )?.["x-no-auth"] === "true"
    );
    if (
      error.response?.status !== 401 ||
      !requiresAuth ||
      originalRequest?._retry
    ) {
      return Promise.reject(error);
    }

    // Queue requests while refreshing
    if (isRefreshing) {
      return new Promise((resolve, reject) => {
        pendingQueue.push({ resolve, reject });
      })
        .then(() => {
          if (originalRequest) {
            (
              originalRequest as InternalAxiosRequestConfig & {
                _retry?: boolean;
              }
            )._retry = true;
          }
          return apiClient(originalRequest!);
        })
        .catch((err) => Promise.reject(err));
    }

    isRefreshing = true;
    try {
      await authStore.getState().refreshTokens();
      processQueue();
      if (originalRequest) {
        (
          originalRequest as InternalAxiosRequestConfig & { _retry?: boolean }
        )._retry = true;
      }
      return apiClient(originalRequest!);
    } catch (refreshErr: unknown) {
      processQueue(refreshErr as AxiosError);
      // logout on refresh failure
      authStore.getState().logout();
      return Promise.reject(refreshErr);
    } finally {
      isRefreshing = false;
    }
  },
);

export default apiClient;
