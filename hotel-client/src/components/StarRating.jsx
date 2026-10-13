import "./StarRating.css";

// Renders gold stars for the hotel's actual Rating (1-5, from HotelResponseDto).
export default function StarRating({ rating, size = "md" }) {
  const rounded = Math.round(rating * 2) / 2; // nearest half-star

  return (
    <span className={`star-rating star-rating--${size}`} aria-label={`${rating.toFixed(1)} out of 5 stars`}>
      {[1, 2, 3, 4, 5].map((n) => {
        const fill = rounded >= n ? 1 : rounded >= n - 0.5 ? 0.5 : 0;
        return (
          <span key={n} className="star-rating__star" aria-hidden="true">
            <span className="star-rating__star-bg">★</span>
            {fill > 0 && (
              <span className="star-rating__star-fill" style={{ width: `${fill * 100}%` }}>
                ★
              </span>
            )}
          </span>
        );
      })}
      <span className="star-rating__value">{rating.toFixed(1)}</span>
    </span>
  );
}
