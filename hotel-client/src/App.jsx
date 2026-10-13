import { Navigate, Route, Routes } from "react-router-dom";
import Navbar from "./components/Navbar";
import ProtectedRoute from "./components/ProtectedRoute";
import AdminRoute from "./components/AdminRoute";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import HotelsPage from "./pages/HotelsPage";
import HotelDetailPage from "./pages/HotelDetailPage";
import BookingDetailsPage from "./pages/BookingDetailsPage";
import MyMessagesPage from "./pages/MyMessagesPage";
import ManageInquiriesPage from "./pages/ManageInquiriesPage";

export default function App() {
  return (
    <>
      <Navbar />
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <HotelsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/hotels/:hotelId"
          element={
            <ProtectedRoute>
              <HotelDetailPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/booking"
          element={
            <ProtectedRoute>
              <BookingDetailsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/messages"
          element={
            <ProtectedRoute>
              <MyMessagesPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/inquiries"
          element={
            <AdminRoute>
              <ManageInquiriesPage />
            </AdminRoute>
          }
        />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </>
  );
}
