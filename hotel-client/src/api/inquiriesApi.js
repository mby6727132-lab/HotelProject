import { axiosClient } from "./axiosClient";

// POST /api/inquiries { hotelId, fullName, message } -> InquiryResponseDto
export function createInquiry({ hotelId, fullName, message }) {
  return axiosClient.post("/inquiries", { hotelId, fullName, message }).then((res) => res.data);
}

// GET /api/inquiries/my -> InquiryResponseDto[] (the current user's own conversations only)
export function getMyInquiries() {
  return axiosClient.get("/inquiries/my").then((res) => res.data);
}

// GET /api/inquiries (Admin only) -> AdminInquiryResponseDto[]
export function getAllInquiries() {
  return axiosClient.get("/inquiries").then((res) => res.data);
}

// POST /api/inquiries/{id}/respond (Admin only) { responseText } -> AdminInquiryResponseDto
export function respondToInquiry(id, responseText) {
  return axiosClient.post(`/inquiries/${id}/respond`, { responseText }).then((res) => res.data);
}
