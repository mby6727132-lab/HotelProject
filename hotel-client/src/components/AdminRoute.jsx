import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

// Client-side gate for the Admin UI shell only - the real enforcement is the
// server's [Authorize(Roles = "Admin")] on the inquiries endpoints. The role
// here comes solely from the JWT claim set at login, never from client input.
export default function AdminRoute({ children }) {
  const { isAuthenticated, user } = useAuth();

  if (!isAuthenticated) return <Navigate to="/login" replace />;
  if (user.role !== "Admin") return <Navigate to="/" replace />;

  return children;
}
