import "./SkeletonCard.css";

// Stylized loading placeholder shown while hotel/room data is being fetched.
export default function SkeletonCard({ count = 3, variant = "hotel" }) {
  return (
    <div className={`skeleton-grid skeleton-grid--${variant}`}>
      {Array.from({ length: count }, (_, i) => (
        <div key={i} className="skeleton-card card">
          <div className="skeleton-card__image skeleton-shimmer" />
          <div className="skeleton-card__body">
            <div className="skeleton-line skeleton-shimmer skeleton-line--title" />
            <div className="skeleton-line skeleton-shimmer skeleton-line--sm" />
            <div className="skeleton-line skeleton-shimmer skeleton-line--full" />
            <div className="skeleton-line skeleton-shimmer skeleton-line--full" />
            <div className="skeleton-card__button skeleton-shimmer" />
          </div>
        </div>
      ))}
    </div>
  );
}
