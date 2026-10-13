import { axiosClient } from "./axiosClient";

// POST /api/payments { roomId, checkIn, checkOut, guestFirstName, guestLastName, guestPhone } -> PaymentResponseDto
// Amount is always computed server-side; the client never sends or trusts a price.
export function createPayment({ roomId, checkIn, checkOut, guestFirstName, guestLastName, guestPhone }) {
  return axiosClient
    .post("/payments", { roomId, checkIn, checkOut, guestFirstName, guestLastName, guestPhone })
    .then((res) => res.data);
}
