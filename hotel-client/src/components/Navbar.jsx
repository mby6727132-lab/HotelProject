import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import "./Navbar.css";

export default function Navbar() {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/login");
  }

  return (
    <header className="navbar">
      <div className="container navbar__inner">
        <Link to="/" className="navbar__brand">
          <span className="navbar__brand-mark">V</span>
          Veritas Hotels
        </Link>

        {isAuthenticated && (
          <nav className="navbar__links">
            <Link to="/messages" className="navbar__link">
              My Messages
            </Link>
            {/* Admin privileges come only from the server's JWT role claim - never from client state */}
            {user.role === "Admin" && (
              <Link to="/admin/inquiries" className="navbar__link">
                Manage Inquiries
              </Link>
            )}
          </nav>
        )}

        {isAuthenticated && (
          <div className="navbar__right">
            <span className="navbar__user">
              Hi, {user.name}
              {user.role === "Admin" && <span className="badge badge-burgundy navbar__badge">Admin</span>}
            </span>
            <button type="button" className="btn btn-outline btn-sm" onClick={handleLogout}>
              Log out
            </button>
          </div>
        )}
      </div>
    </header>
  );
}
