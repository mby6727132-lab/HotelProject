import axios from "axios";

const baseURL = import.meta.env.VITE_API_BASE_URL || "http://localhost:65434/api";

export const axiosClient = axios.create({
  baseURL,
  headers: {
    "Content-Type": "application/json",
  },
});

const TOKEN_STORAGE_KEY = "hotel_auth";

export function getStoredAuth() {
  try {
    const raw = localStorage.getItem(TOKEN_STORAGE_KEY);
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
}

export function storeAuth(auth) {
  localStorage.setItem(TOKEN_STORAGE_KEY, JSON.stringify(auth));
}

export function clearStoredAuth() {
  localStorage.removeItem(TOKEN_STORAGE_KEY);
}

// Attach the JWT (if present) to every outgoing request, centrally.
axiosClient.interceptors.request.use((config) => {
  const auth = getStoredAuth();
  if (auth?.token) {
    config.headers.Authorization = `Bearer ${auth.token}`;
  }
  return config;
});

// Registered by AuthContext so a 401 anywhere can log the user out centrally,
// without axiosClient importing AuthContext directly.
let unauthorizedHandler = null;
export function setUnauthorizedHandler(handler) {
  unauthorizedHandler = handler;
}

axiosClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401 && unauthorizedHandler) {
      unauthorizedHandler();
    }
    return Promise.reject(error);
  }
);

// Best-effort extraction of a human-readable message from any of the API's
// error response shapes (custom {status,message,correlationId}, ad-hoc
// {message}, or the framework's default ProblemDetails {title}).
export function getErrorMessage(error, fallback = "Something went wrong. Please try again.") {
  const data = error?.response?.data;
  if (typeof data === "string" && data.trim()) return data;
  if (data?.message) return data.message;
  if (data?.title) return data.title;
  if (error?.message) return error.message;
  return fallback;
}

export function getStatus(error) {
  return error?.response?.status;
}
