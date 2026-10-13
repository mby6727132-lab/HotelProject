import { axiosClient } from "./axiosClient";

// GET /api/hotels?city=...&name=... -> HotelResponseDto[]
export function getHotelsByCity(city, name) {
  return axiosClient.get("/hotels", { params: { city, name: name || undefined } }).then((res) => res.data);
}

// GET /api/hotels/{id} -> HotelResponseDto
export function getHotelById(id) {
  return axiosClient.get(`/hotels/${id}`).then((res) => res.data);
}
