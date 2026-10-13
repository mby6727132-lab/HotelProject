import { useState } from "react";
import { Navigate, useLocation, useNavigate } from "react-router-dom";
import { createPayment } from "../api/paymentsApi";
import { getErrorMessage, getStatus } from "../api/axiosClient";
import { useAuth } from "../context/AuthContext";
import { formatCurrency } from "../utils/currency";
import ImageGallery from "../components/ImageGallery";
import { roomCategory } from "../components/RoomCard";
import Alert from "../components/Alert";
import "./BookingDetailsPage.css";

function nightsBetween(checkIn, checkOut) {
  const ms = new Date(checkOut) - new Date(checkIn);
  return Math.max(0, Math.round(ms / (1000 * 60 * 60 * 24)));
}

function splitName(fullName = "") {
  const parts = fullName.trim().split(/\s+/);
  return { firstName: parts[0] ?? "", lastName: parts.slice(1).join(" ") };
}

export default function BookingDetailsPage() {
  const location = useLocation();
  const navigate = useNavigate();
  const { user } = useAuth();
  const booking = location.state;

  const { firstName: initialFirst, lastName: initialLast } = splitName(user?.name);
  const [form, setForm] = useState({ firstName: initialFirst, lastName: initialLast, phone: "" });
  const [fieldErrors, setFieldErrors] = useState({});
  const [status, setStatus] = useState("form"); // form | submitting | success | conflict | error
  const [errorMessage, setErrorMessage] = useState("");
  const [confirmedPayment, setConfirmedPayment] = useState(null);

  // Guard: this page only makes sense arriving from the Rooms page with a selection in hand.
  if (!booking?.hotel || !booking?.room) {
    return <Navigate to="/" replace />;
  }

  const { hotel, room, checkIn, checkOut, guests } = booking;
  const nights = nightsBetween(checkIn, checkOut);
  const totalCost = nights * room.price;

  function validate() {
    const errors = {};
    if (!form.firstName.trim()) errors.firstName = "First name is required.";
    if (!form.lastName.trim()) errors.lastName = "Last name is required.";
    if (!form.phone.trim() || form.phone.trim().length < 7) errors.phone = "Enter a valid mobile number.";
    setFieldErrors(errors);
    return Object.keys(errors).length === 0;
  }

  async function handleSubmit(e) {
    e.preventDefault();
    if (!validate()) return;

    setStatus("submitting");
    setErrorMessage("");
    try {
      const payment = await createPayment({
        roomId: room.id,
        checkIn,
        checkOut,
        guestFirstName: form.firstName.trim(),
        guestLastName: form.lastName.trim(),
        guestPhone: form.phone.trim(),
      });
      setConfirmedPayment(payment);
      setStatus("success");
    } catch (err) {
      if (getStatus(err) === 409) {
        setStatus("conflict");
      } else {
        setErrorMessage(getErrorMessage(err, "We couldn't complete your booking. Please try again."));
        setStatus("error");
      }
    }
  }

  if (status === "success") {
    return (
      <div className="page booking-result">
        <div className="container booking-result__inner">
          <div className="card booking-result__card">
            <div className="booking-result__icon">✓</div>
            <h1>Booking Confirmed!</h1>
            <p>Thank you, {confirmedPayment.guestFirstName}. Your stay at {hotel.name} is booked.</p>

            <dl className="booking-summary">
              <div>
                <dt>Room</dt>
                <dd>{room.name || `Room ${room.roomNumber}`}</dd>
              </div>
              <div>
                <dt>Check-in</dt>
                <dd>{confirmedPayment.checkIn.slice(0, 10)}</dd>
              </div>
              <div>
                <dt>Check-out</dt>
                <dd>{confirmedPayment.checkOut.slice(0, 10)}</dd>
              </div>
              <div>
                <dt>Total order cost</dt>
                <dd className="booking-summary__total">{formatCurrency(confirmedPayment.amount)}</dd>
              </div>
            </dl>

            <button type="button" className="btn btn-primary btn-block" onClick={() => navigate("/")}>
              Back to Home
            </button>
          </div>
        </div>
      </div>
    );
  }

  if (status === "conflict") {
    return (
      <div className="page booking-result">
        <div className="container booking-result__inner">
          <div className="card booking-result__card">
            <div className="booking-result__icon booking-result__icon--warning">!</div>
            <h1>Room No Longer Available</h1>
            <p>The room you selected is no longer available. Please choose another room.</p>
            <button
              type="button"
              className="btn btn-primary btn-block"
              onClick={() => navigate(`/hotels/${hotel.id}/rooms`, { state: { hotel } })}
            >
              Choose Another Room
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="page">
      <section className="section">
        <div className="container">
          <h1 className="booking-details__title">Complete Your Booking</h1>

          <div className="booking-details__layout">
            <form className="card booking-details__form" onSubmit={handleSubmit} noValidate>
              <h2>Guest Details</h2>

              {status === "error" && <Alert type="error">{errorMessage}</Alert>}

              <div className="field">
                <label htmlFor="firstName">First Name</label>
                <input
                  id="firstName"
                  type="text"
                  className={fieldErrors.firstName ? "has-error" : ""}
                  value={form.firstName}
                  onChange={(e) => setForm({ ...form, firstName: e.target.value })}
                />
                {fieldErrors.firstName && <span className="field-error">{fieldErrors.firstName}</span>}
              </div>

              <div className="field">
                <label htmlFor="lastName">Last Name</label>
                <input
                  id="lastName"
                  type="text"
                  className={fieldErrors.lastName ? "has-error" : ""}
                  value={form.lastName}
                  onChange={(e) => setForm({ ...form, lastName: e.target.value })}
                />
                {fieldErrors.lastName && <span className="field-error">{fieldErrors.lastName}</span>}
              </div>

              <div className="field">
                <label htmlFor="phone">Mobile Number</label>
                <input
                  id="phone"
                  type="tel"
                  placeholder="+972-50-1234567"
                  className={fieldErrors.phone ? "has-error" : ""}
                  value={form.phone}
                  onChange={(e) => setForm({ ...form, phone: e.target.value })}
                />
                {fieldErrors.phone && <span className="field-error">{fieldErrors.phone}</span>}
              </div>

              <button type="submit" className="btn btn-primary btn-block" disabled={status === "submitting"}>
                {status === "submitting" ? "Confirming Booking..." : "Confirm Booking"}
              </button>
            </form>

            <aside className="card booking-details__summary">
              <ImageGallery category={roomCategory(room.name)} alt={room.name} compact />

              <h2>{hotel.name}</h2>
              <p className="booking-details__room-name">{room.name || `Room ${room.roomNumber}`}</p>

              <dl className="booking-summary">
                <div>
                  <dt>Check-in</dt>
                  <dd>{checkIn}</dd>
                </div>
                <div>
                  <dt>Check-out</dt>
                  <dd>{checkOut}</dd>
                </div>
                <div>
                  <dt>Nights</dt>
                  <dd>{nights}</dd>
                </div>
                <div>
                  <dt>Guests</dt>
                  <dd>{guests}</dd>
                </div>
                <div>
                  <dt>Price per night</dt>
                  <dd>{formatCurrency(room.price)}</dd>
                </div>
              </dl>

              <div className="booking-details__totals">
                <div className="booking-details__total-row booking-details__total-row--main">
                  <span>Total order cost</span>
                  <span>{formatCurrency(totalCost)}</span>
                </div>
                <div className="booking-details__total-row">
                  <span>Total due now</span>
                  <span>{formatCurrency(0)}</span>
                </div>
                <p className="booking-details__pay-note">Payment for the vacation is made at the hotel.</p>
              </div>
            </aside>
          </div>
        </div>
      </section>
    </div>
  );
}
