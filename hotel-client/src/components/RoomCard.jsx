import ImageGallery from "./ImageGallery";
import { formatCurrency } from "../utils/currency";
import "./RoomCard.css";

export function roomCategory(name = "") {
  const lower = name.toLowerCase();
  if (lower.includes("suite")) return "suite";
  if (lower.includes("family")) return "family";
  if (lower.includes("deluxe")) return "deluxe";
  if (lower.includes("standard")) return "standard";
  return "room";
}

// Renders only fields that actually exist on RoomResponseDto.
export default function RoomCard({ room, onSelect }) {
  const amenities = room.amenities ? room.amenities.split(",").filter(Boolean) : [];

  return (
    <article className="room-card card">
      <ImageGallery category={roomCategory(room.name)} alt={room.name} compact />

      <div className="room-card__body">
        <div className="room-card__header">
          <h3 className="room-card__name">{room.name || `Room ${room.roomNumber}`}</h3>
          <span className="badge badge-navy">👤 {room.capacity}</span>
        </div>

        <p className="room-card__meta">
          Room {room.roomNumber} · Floor {room.floor}
        </p>

        {room.description && <p className="room-card__description">{room.description}</p>}

        {amenities.length > 0 && (
          <ul className="room-card__amenities">
            {amenities.slice(0, 3).map((a) => (
              <li key={a}>{a}</li>
            ))}
            {amenities.length > 3 && <li className="room-card__amenities-more">+{amenities.length - 3}</li>}
          </ul>
        )}

        <div className="room-card__price">
          <span className="room-card__price-amount">{formatCurrency(room.price)}</span>
          <span className="room-card__price-unit">/ night</span>
        </div>

        <button type="button" className="btn btn-primary btn-block" onClick={() => onSelect(room)}>
          Select This Room
        </button>
      </div>
    </article>
  );
}
