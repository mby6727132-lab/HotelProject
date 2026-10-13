import { useState } from "react";
import { useNavigate } from "react-router-dom";
import PlaceholderPhoto from "./PlaceholderPhoto";
import StarRating from "./StarRating";
import { getMainPhoto } from "../utils/stockPhotos";
import "./HotelCard.css";

// Renders only fields that actually exist on HotelResponseDto.
export default function HotelCard({ hotel }) {
  const navigate = useNavigate();
  const [imageFailed, setImageFailed] = useState(false);
  const amenities = hotel.amenities ? hotel.amenities.split(",").filter(Boolean) : [];

  return (
    <article className="hotel-card card">
      <div className="hotel-card__image">
        {imageFailed ? (
          <PlaceholderPhoto seed={`hotel-${hotel.id}`} index={0} category="hotel" className="hotel-card__image-art" />
        ) : (
          <img
            src={getMainPhoto("hotel")}
            alt={hotel.name}
            className="hotel-card__image-art"
            loading="lazy"
            onError={() => setImageFailed(true)}
          />
        )}
        <span className="badge badge-navy hotel-card__city-badge">{hotel.city}</span>
        <span className="hotel-card__stock-badge">📷 Stock photo</span>
      </div>

      <div className="hotel-card__body">
        <div className="hotel-card__title-row">
          <h3 className="hotel-card__name">{hotel.name}</h3>
          <StarRating rating={hotel.rating} size="sm" />
        </div>
        <p className="hotel-card__address">📍 {hotel.address}</p>

        {hotel.style && <span className="badge badge-burgundy hotel-card__style">{hotel.style}</span>}

        {hotel.description && <p className="hotel-card__description">{hotel.description}</p>}

        {amenities.length > 0 && (
          <ul className="hotel-card__amenities">
            {amenities.slice(0, 4).map((a) => (
              <li key={a}>{a}</li>
            ))}
            {amenities.length > 4 && <li className="hotel-card__amenities-more">+{amenities.length - 4} more</li>}
          </ul>
        )}

        {hotel.targetAudience && <p className="hotel-card__audience">Ideal for: {hotel.targetAudience}</p>}

        <button
          type="button"
          className="btn btn-primary btn-block hotel-card__cta"
          onClick={() => navigate(`/hotels/${hotel.id}`, { state: { hotel } })}
        >
          View Hotel &amp; Select Room
        </button>
      </div>
    </article>
  );
}
