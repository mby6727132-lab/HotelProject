import { useState } from "react";
import { createInquiry } from "../api/inquiriesApi";
import { getErrorMessage } from "../api/axiosClient";
import { useAuth } from "../context/AuthContext";
import Alert from "./Alert";
import "./ContactHotelModal.css";

export default function ContactHotelModal({ hotel, onClose }) {
  const { user } = useAuth();
  const [fullName, setFullName] = useState(user?.name ?? "");
  const [message, setMessage] = useState("");
  const [fieldErrors, setFieldErrors] = useState({});
  const [status, setStatus] = useState("form"); // form | submitting | success | error
  const [errorMessage, setErrorMessage] = useState("");

  function validate() {
    const errors = {};
    if (!fullName.trim()) errors.fullName = "Full name is required.";
    if (!message.trim()) errors.message = "Please enter a message.";
    setFieldErrors(errors);
    return Object.keys(errors).length === 0;
  }

  async function handleSubmit(e) {
    e.preventDefault();
    if (!validate()) return;

    setStatus("submitting");
    setErrorMessage("");
    try {
      await createInquiry({ hotelId: hotel.id, fullName: fullName.trim(), message: message.trim() });
      setStatus("success");
    } catch (err) {
      setErrorMessage(getErrorMessage(err, "We couldn't send your message. Please try again."));
      setStatus("error");
    }
  }

  return (
    <div className="modal-overlay" role="dialog" aria-modal="true" onClick={onClose}>
      <div className="modal" onClick={(e) => e.stopPropagation()}>
        <button type="button" className="modal__close" onClick={onClose} aria-label="Close">
          ×
        </button>

        {status === "success" ? (
          <div className="contact-modal__success">
            <div className="contact-modal__success-icon">✓</div>
            <h2>Message Sent</h2>
            <p>Your message has been sent successfully! You can view the reply in the "My Messages" area.</p>
            <button type="button" className="btn btn-primary btn-block" onClick={onClose}>
              Close
            </button>
          </div>
        ) : (
          <>
            <h2>Contact {hotel.name}</h2>

            {status === "error" && <Alert type="error">{errorMessage}</Alert>}

            <form onSubmit={handleSubmit} noValidate>
              <div className="field">
                <label htmlFor="contact-fullName">Full Name</label>
                <input
                  id="contact-fullName"
                  type="text"
                  className={fieldErrors.fullName ? "has-error" : ""}
                  value={fullName}
                  onChange={(e) => setFullName(e.target.value)}
                />
                {fieldErrors.fullName && <span className="field-error">{fieldErrors.fullName}</span>}
              </div>

              <div className="field">
                <label htmlFor="contact-message">Message</label>
                <textarea
                  id="contact-message"
                  rows={6}
                  className={fieldErrors.message ? "has-error" : ""}
                  value={message}
                  onChange={(e) => setMessage(e.target.value)}
                />
                {fieldErrors.message && <span className="field-error">{fieldErrors.message}</span>}
              </div>

              <button type="submit" className="btn btn-primary btn-block" disabled={status === "submitting"}>
                {status === "submitting" ? "Sending..." : "Send Message"}
              </button>
            </form>
          </>
        )}
      </div>
    </div>
  );
}
