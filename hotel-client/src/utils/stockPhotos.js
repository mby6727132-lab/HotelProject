import hotelExterior from "../assets/stock-photos/hotel-exterior.jpg";
import roomStandard from "../assets/stock-photos/room-standard.jpg";
import roomLiving from "../assets/stock-photos/room-living.jpg";

// Real, verified (visually inspected - no people), royalty-free photos from
// Wikimedia Commons, bundled locally for stable loading. There isn't a
// licensed, verified photo for each specific fictional hotel/room in this
// demo, so these are reused as clearly-labeled stock photography rather than
// presented as a shot of any particular property.
export const STOCK_PHOTOS = {
  hotel: [hotelExterior, roomStandard, roomLiving],
  standard: [roomStandard],
  deluxe: [roomLiving],
  family: [roomLiving],
  suite: [roomLiving],
  room: [roomStandard],
};

export function getStockPhotos(category = "hotel") {
  return STOCK_PHOTOS[category] ?? STOCK_PHOTOS.hotel;
}

export function getMainPhoto(category = "hotel") {
  return getStockPhotos(category)[0];
}
