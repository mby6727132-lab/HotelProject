import { axiosClient } from "./axiosClient";

// POST /api/auth/register -> AuthResponseDto { token, name, role }
export function register({ name, email, password }) {
  return axiosClient.post("/auth/register", { name, email, password }).then((res) => res.data);
}

// POST /api/auth/login -> AuthResponseDto { token, name, role }
export function login({ email, password }) {
  return axiosClient.post("/auth/login", { email, password }).then((res) => res.data);
}
