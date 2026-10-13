import { useNavigate } from "react-router-dom";
import { getMainPhoto } from "../utils/stockPhotos";
import { formatCurrency } from "../utils/currency";
import "./DealCard.css";

// Minimal deal card: image, hotel name, price for a couple - nothing else
// (no description/amenities/rating here, by design). The whole card is
// clickable and opens the hotel's full details page.
export default function DealCard({ hotel }) {
  const navigate = useNavigate();

  function open() {
    navigate(`/hotels/${hotel.id}`, { state: { hotel } });
  }

  return (
    <article className="deal-card card" onClick={open} role="link" tabIndex={0}
      onKeyDown={(e) => (e.key === "Enter" || e.key === " ") && open()}>
      <div className="deal-card__image">
        <img src={getMainPhoto("hotel")} alt={hotel.name} loading="lazy" />
        <span className="deal-card__stock-badge">📷 Stock photo</span>
      </div>
      <div className="deal-card__body">
        <h3 className="deal-card__name">{hotel.name}</h3>
        {hotel.priceForCouple != null && (
          <p className="deal-card__price">
            {formatCurrency(hotel.priceForCouple)} <span>/ night for 2</span>
          </p>
        )}
      </div>
    </article>
  );
}
