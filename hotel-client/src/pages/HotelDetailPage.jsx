import { useEffect, useRef, useState } from "react";
import { Link, useLocation, useParams } from "react-router-dom";
import { getHotelById } from "../api/hotelsApi";
import ImageGallery from "../components/ImageGallery";
import StarRating from "../components/StarRating";
import RoomsSection from "../components/RoomsSection";
import ContactHotelModal from "../components/ContactHotelModal";
import LoadingSpinner from "../components/LoadingSpinner";
import Alert from "../components/Alert";
import { getErrorMessage } from "../api/axiosClient";
import "./HotelDetailPage.css";

const NAV_SECTIONS = [
  { id: "gallery", label: "Gallery" },
  { id: "about", label: "About" },
  { id: "amenities", label: "Amenities" },
  { id: "location", label: "Location" },
  { id: "rooms", label: "Rooms" },
];

export default function HotelDetailPage() {
  const { hotelId } = useParams();
  const location = useLocation();

  const [hotel, setHotel] = useState(location.state?.hotel ?? null);
  const [status, setStatus] = useState(location.state?.hotel ? "loaded" : "loading");
  const [errorMessage, setErrorMessage] = useState("");
  const [contactOpen, setContactOpen] = useState(false);
  const [activeSection, setActiveSection] = useState("gallery");
  const isClickScrolling = useRef(false);

  useEffect(() => {
    if (hotel) return;
    getHotelById(hotelId)
      .then((data) => {
        setHotel(data);
        setStatus("loaded");
      })
      .catch((err) => {
        setErrorMessage(getErrorMessage(err, "Could not load this hotel."));
        setStatus("error");
      });
  }, [hotel, hotelId]);

  // Scroll-spy: highlight whichever section is currently in view as the user
  // scrolls, not just on click.
  useEffect(() => {
    if (status !== "loaded") return;

    const observer = new IntersectionObserver(
      (entries) => {
        if (isClickScrolling.current) return;
        const visible = entries.filter((e) => e.isIntersecting);
        if (visible.length > 0) {
          const topMost = visible.reduce((a, b) => (a.boundingClientRect.top < b.boundingClientRect.top ? a : b));
          setActiveSection(topMost.target.id);
        }
      },
      { rootMargin: "-160px 0px -60% 0px", threshold: 0 }
    );

    NAV_SECTIONS.forEach((s) => {
      const el = document.getElementById(s.id);
      if (el) observer.observe(el);
    });

    return () => observer.disconnect();
  }, [status]);

  function scrollToSection(id) {
    setActiveSection(id);
    isClickScrolling.current = true;
    document.getElementById(id)?.scrollIntoView({ behavior: "smooth", block: "start" });
    window.setTimeout(() => {
      isClickScrolling.current = false;
    }, 700);
  }

  if (status === "loading") return <LoadingSpinner label="Loading hotel..." />;
  if (status === "error") {
    return (
      <div className="container section">
        <Alert type="error">{errorMessage}</Alert>
      </div>
    );
  }

  const amenities = hotel.amenities ? hotel.amenities.split(",").filter(Boolean) : [];

  return (
    <div className="page hotel-detail">
      <div className="container">
        <Link to="/" className="hotel-detail__back">
          ← Back to search results
        </Link>

        <div className="hotel-detail__header">
          <div>
            <span className="badge badge-navy">{hotel.city}</span>
            <h1>{hotel.name}</h1>
            <StarRating rating={hotel.rating} />
          </div>
        </div>
      </div>

      <nav className="hotel-nav" aria-label="Hotel page sections">
        <div className="container hotel-nav__inner">
          {NAV_SECTIONS.map((s) => (
            <button
              key={s.id}
              type="button"
              className={`hotel-nav__item ${activeSection === s.id ? "is-active" : ""}`}
              onClick={() => scrollToSection(s.id)}
            >
              {s.label}
            </button>
          ))}
        </div>
      </nav>

      <div className="container hotel-detail__sections">
        <section id="gallery" className="hotel-detail__section">
          <ImageGallery category="hotel" alt={hotel.name} />
        </section>

        <section id="about" className="card hotel-detail__section">
          <h2>About {hotel.name}</h2>
          {hotel.style && <span className="badge badge-burgundy hotel-detail__style">{hotel.style}</span>}
          <p>{hotel.description}</p>
          {hotel.targetAudience && (
            <p className="hotel-detail__audience">
              <strong>Perfect for:</strong> {hotel.targetAudience}
            </p>
          )}
        </section>

        <section id="amenities" className="card hotel-detail__section">
          <h2>Amenities &amp; Services</h2>
          {amenities.length > 0 ? (
            <ul className="hotel-detail__amenities">
              {amenities.map((a) => (
                <li key={a}>
                  <span className="hotel-detail__amenity-icon" aria-hidden="true">
                    ✓
                  </span>
                  {a}
                </li>
              ))}
            </ul>
          ) : (
            <p>No amenities listed.</p>
          )}
        </section>

        <section id="location" className="card hotel-detail__section">
          <h2>Location</h2>
          <p>
            <strong>{hotel.city}</strong>
          </p>
          <p>{hotel.address}</p>
          <p className="hotel-detail__audience">
            Located in {hotel.city}, close to the area's main attractions, dining, and transport links.
          </p>
        </section>

        <section id="rooms" className="hotel-detail__section">
          <h2>Rooms</h2>
          <RoomsSection hotel={hotel} />
        </section>
      </div>

      <button
        type="button"
        className="hotel-detail__fab"
        onClick={() => setContactOpen(true)}
        aria-label="Contact this hotel"
      >
        <span aria-hidden="true">💬</span> Contact
      </button>

      {contactOpen && <ContactHotelModal hotel={hotel} onClose={() => setContactOpen(false)} />}
    </div>
  );
}
