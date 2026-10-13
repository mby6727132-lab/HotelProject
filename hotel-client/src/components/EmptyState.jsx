import "./EmptyState.css";

export default function EmptyState({ icon = "🔍", title, description }) {
  return (
    <div className="empty-state">
      <div className="empty-state__icon" aria-hidden="true">
        {icon}
      </div>
      <h3>{title}</h3>
      {description && <p>{description}</p>}
    </div>
  );
}
