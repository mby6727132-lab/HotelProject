import { axiosClient } from "./axiosClient";

// GET /api/hotels/{hotelId}/rooms/available?checkIn=&checkOut=&guests=&page=&pageSize= -> RoomResponseDto[]
export function getAvailableRooms(hotelId, { checkIn, checkOut, guests, page, pageSize }) {
  return axiosClient
    .get(`/hotels/${hotelId}/rooms/available`, {
      params: { checkIn, checkOut, guests: guests || undefined, page, pageSize },
    })
    .then((res) => res.data);
}
