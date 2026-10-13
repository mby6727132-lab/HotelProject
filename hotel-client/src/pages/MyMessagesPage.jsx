import { useEffect, useState } from "react";
import { getMyInquiries } from "../api/inquiriesApi";
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

export default function MyMessagesPage() {
  const [inquiries, setInquiries] = useState([]);
  const [status, setStatus] = useState("loading");
  const [errorMessage, setErrorMessage] = useState("");
  const [selectedId, setSelectedId] = useState(null);

  useEffect(() => {
    getMyInquiries()
      .then((data) => {
        setInquiries(data);
        setSelectedId(data[0]?.id ?? null);
        setStatus("loaded");
      })
      .catch((err) => {
        setErrorMessage(getErrorMessage(err, "Could not load your messages."));
        setStatus("error");
      });
  }, []);

  if (status === "loading") return <LoadingSpinner label="Loading your messages..." />;
  if (status === "error") {
    return (
      <div className="container section">
        <Alert type="error">{errorMessage}</Alert>
      </div>
    );
  }

  const selected = inquiries.find((i) => i.id === selectedId);

  return (
    <div className="page">
      <section className="section">
        <div className="container">
          <h1 className="messages-title">My Messages</h1>

          {inquiries.length === 0 ? (
            <EmptyState
              icon="💬"
              title="No conversations yet"
              description="Use the 'Contact Hotel' button on any hotel page to start a conversation."
            />
          ) : (
            <div className="messages-layout card">
              <aside className="messages-list">
                {inquiries.map((inquiry) => (
                  <button
                    key={inquiry.id}
                    type="button"
                    className={`messages-list__item ${inquiry.id === selectedId ? "is-active" : ""}`}
                    onClick={() => setSelectedId(inquiry.id)}
                  >
                    <span className="messages-list__hotel">{inquiry.hotelName}</span>
                    <span className="messages-list__preview">{inquiry.message}</span>
                    <span className={`badge ${inquiry.status === "Responded" ? "badge-navy" : "badge-burgundy"}`}>
                      {inquiry.status === "Responded" ? "Responded" : "Awaiting response"}
                    </span>
                  </button>
                ))}
              </aside>

              {selected && (
                <div className="messages-thread">
                  <div className="messages-thread__header">
                    <h2>{selected.hotelName}</h2>
                    <span className={`badge ${selected.status === "Responded" ? "badge-navy" : "badge-burgundy"}`}>
                      {selected.status === "Responded" ? "Responded" : "Awaiting response"}
                    </span>
                  </div>

                  <div className="messages-thread__body">
                    <div className="message-bubble message-bubble--user">
                      <p>{selected.message}</p>
                      <span className="message-bubble__time">{formatDateTime(selected.createdAt)}</span>
                    </div>

                    {selected.responseText ? (
                      <div className="message-bubble message-bubble--hotel">
                        <span className="message-bubble__sender">{selected.hotelName}</span>
                        <p>{selected.responseText}</p>
                        <span className="message-bubble__time">{formatDateTime(selected.respondedAt)}</span>
                      </div>
                    ) : (
                      <p className="messages-thread__waiting">Awaiting a response from the hotel.</p>
                    )}
                  </div>
                </div>
              )}
            </div>
          )}
        </div>
      </section>
    </div>
  );
}
