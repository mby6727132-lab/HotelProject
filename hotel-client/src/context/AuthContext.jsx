import { createContext, useCallback, useContext, useEffect, useMemo, useState } from "react";
import * as authApi from "../api/authApi";
import { clearStoredAuth, getStoredAuth, setUnauthorizedHandler, storeAuth } from "../api/axiosClient";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [auth, setAuth] = useState(() => getStoredAuth());

  const logout = useCallback(() => {
    clearStoredAuth();
    setAuth(null);
  }, []);

  // Any 401 from the API (missing/expired token) logs the user out centrally.
  useEffect(() => {
    setUnauthorizedHandler(logout);
  }, [logout]);

  const login = useCallback(async (email, password) => {
    const result = await authApi.login({ email, password });
    const nextAuth = { token: result.token, name: result.name, role: result.role };
    storeAuth(nextAuth);
    setAuth(nextAuth);
    return nextAuth;
  }, []);

  const register = useCallback(async (name, email, password) => {
    const result = await authApi.register({ name, email, password });
    const nextAuth = { token: result.token, name: result.name, role: result.role };
    storeAuth(nextAuth);
    setAuth(nextAuth);
    return nextAuth;
  }, []);

  const value = useMemo(
    () => ({
      user: auth ? { name: auth.name, role: auth.role } : null,
      token: auth?.token ?? null,
      isAuthenticated: Boolean(auth?.token),
      login,
      register,
      logout,
    }),
    [auth, login, register, logout]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within an AuthProvider");
  return ctx;
}
