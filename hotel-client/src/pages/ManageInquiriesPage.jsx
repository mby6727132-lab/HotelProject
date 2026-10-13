import { useEffect, useState } from "react";
import { getAllInquiries, respondToInquiry } from "../api/inquiriesApi";
import { getErrorMessage } from "../api/axiosClient";
import LoadingSpinner from "../components/LoadingSpinner";
import EmptyState from "../components/EmptyState";
import Alert from "../components/Alert";
import "./MessagesPage.css";

function formatDateTime(value) {
  return new Date(value).toLocaleString(undefined, {
    year: "numeric",
    month: "short",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
}

export default function ManageInquiriesPage() {
  const [inquiries, setInquiries] = useState([]);
  const [status, setStatus] = useState("loading");
  const [errorMessage, setErrorMessage] = useState("");
  const [selectedId, setSelectedId] = useState(null);
  const [responseText, setResponseText] = useState("");
  const [sendStatus, setSendStatus] = useState("idle"); // idle | sending | sent | error
  const [sendError, setSendError] = useState("");

  function loadInquiries() {
    setStatus("loading");
    getAllInquiries()
      .then((data) => {
        setInquiries(data);
        setSelectedId((current) => current ?? data[0]?.id ?? null);
        setStatus("loaded");
      })
      .catch((err) => {
        setErrorMessage(getErrorMessage(err, "Could not load inquiries."));
        setStatus("error");
      });
  }

  useEffect(loadInquiries, []);

  const selected = inquiries.find((i) => i.id === selectedId);

  async function handleRespond(e) {
    e.preventDefault();
    if (!responseText.trim() || !selected) return;

    setSendStatus("sending");
    setSendError("");
    try {
      const updated = await respondToInquiry(selected.id, responseText.trim());
      setInquiries((prev) => prev.map((i) => (i.id === updated.id ? updated : i)));
      setResponseText("");
      setSendStatus("sent");
    } catch (err) {
      setSendError(getErrorMessage(err, "Could not send the response."));
      setSendStatus("error");
    }
  }

  function selectInquiry(id) {
    setSelectedId(id);
    setResponseText("");
    setSendStatus("idle");
  }

  if (status === "loading") return <LoadingSpinner label="Loading inquiries..." />;
  if (status === "error") {
    return (
      <div className="container section">
        <Alert type="error">{errorMessage}</Alert>
      </div>
    );
  }

  return (
    <div className="page">
      <section className="section">
        <div className="container">
          <h1 className="messages-title">Manage Inquiries</h1>

          {inquiries.length === 0 ? (
            <EmptyState icon="📭" title="No inquiries yet" description="Guest messages will appear here." />
          ) : (
            <div className="messages-layout card">
              <aside className="messages-list">
                {inquiries.map((inquiry) => (
                  <button
                    key={inquiry.id}
                    type="button"
                    className={`messages-list__item ${inquiry.id === selectedId ? "is-active" : ""}`}
                    onClick={() => selectInquiry(inquiry.id)}
                  >
                    <span className="messages-list__hotel">{inquiry.hotelName}</span>
                    <span className="messages-list__preview">
                      {inquiry.userName} · {inquiry.message}
                    </span>
                    <span className={`badge ${inquiry.status === "Responded" ? "badge-navy" : "badge-burgundy"}`}>
                      {inquiry.status === "Responded" ? "Responded" : "Awaiting response"}
                    </span>
                  </button>
                ))}
              </aside>

              {selected && (
                <div className="messages-thread">
                  <div className="messages-thread__header">
                    <div>
                      <h2>{selected.hotelName}</h2>
                      <p className="manage-inquiries__meta">
                        From {selected.userName} ({selected.userEmail}) · {formatDateTime(selected.createdAt)}
                      </p>
                    </div>
                    <span className={`badge ${selected.status === "Responded" ? "badge-navy" : "badge-burgundy"}`}>
                      {selected.status === "Responded" ? "Responded" : "Awaiting response"}
                    </span>
                  </div>

                  <div className="messages-thread__body">
                    <div className="message-bubble message-bubble--user">
                      <span className="message-bubble__sender">{selected.fullName}</span>
                      <p>{selected.message}</p>
                      <span className="message-bubble__time">{formatDateTime(selected.createdAt)}</span>
                    </div>

                    {selected.responseText && (
                      <div className="message-bubble message-bubble--hotel">
                        <span className="message-bubble__sender">Your Response</span>
                        <p>{selected.responseText}</p>
                        <span className="message-bubble__time">{formatDateTime(selected.respondedAt)}</span>
                      </div>
                    )}
                  </div>

                  <form className="manage-inquiries__form" onSubmit={handleRespond}>
                    {sendStatus === "sent" && <Alert type="success">Response sent successfully.</Alert>}
                    {sendStatus === "error" && <Alert type="error">{sendError}</Alert>}

                    <div className="field">
                      <label htmlFor="response">{selected.responseText ? "Send another response" : "Write a response"}</label>
                      <textarea
                        id="response"
                        rows={4}
                        value={responseText}
                        onChange={(e) => setResponseText(e.target.value)}
                      />
                    </div>
                    <button type="submit" className="btn btn-primary" disabled={sendStatus === "sending" || !responseText.trim()}>
                      {sendStatus === "sending" ? "Sending..." : "Send Response"}
                    </button>
                  </form>
                </div>
              )}
            </div>
          )}
        </div>
      </section>
    </div>
  );
}
