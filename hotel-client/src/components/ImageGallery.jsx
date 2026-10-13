import { useState } from "react";
import { getStockPhotos } from "../utils/stockPhotos";
import PlaceholderPhoto from "./PlaceholderPhoto";
import "./ImageGallery.css";

// A self-contained gallery: large main image, prev/next arrows, dot
// indicators, smooth transitions. Images are real, verified (no people)
// stock photography (see utils/stockPhotos) - clearly labeled "Stock photo"
// since none of them are a licensed photo of this specific fictional hotel.
// If an image ever fails to load, a designed placeholder is shown instead
// of a broken-image icon.
export default function ImageGallery({ category = "hotel", alt = "", compact = false }) {
  const images = getStockPhotos(category);
  const [index, setIndex] = useState(0);
  const [failed, setFailed] = useState(() => new Set());

  function goTo(nextIndex) {
    setIndex((nextIndex + images.length) % images.length);
  }

  function markFailed(i) {
    setFailed((prev) => new Set(prev).add(i));
  }

  const showPlaceholder = failed.has(index);

  return (
    <div className={`image-gallery ${compact ? "image-gallery--compact" : ""}`}>
      <div className="image-gallery__viewport">
        {showPlaceholder ? (
          <PlaceholderPhoto seed={category} index={index} category={category} className="image-gallery__image" />
        ) : (
          <img
            key={images[index]}
            src={images[index]}
            alt={alt ? `${alt} - photo ${index + 1}` : `Photo ${index + 1}`}
            className="image-gallery__image"
            loading="lazy"
            onError={() => markFailed(index)}
          />
        )}

        <span className="image-gallery__stock-badge">📷 Stock photo</span>

        {images.length > 1 && (
          <>
            <button
              type="button"
              className="image-gallery__arrow image-gallery__arrow--left"
              onClick={() => goTo(index - 1)}
              aria-label="Previous photo"
            >
              ‹
            </button>
            <button
              type="button"
              className="image-gallery__arrow image-gallery__arrow--right"
              onClick={() => goTo(index + 1)}
              aria-label="Next photo"
            >
              ›
            </button>
            <span className="image-gallery__counter">
              {index + 1} / {images.length}
            </span>
          </>
        )}
      </div>

      {images.length > 1 && (
        <div className="image-gallery__dots">
          {images.map((url, i) => (
            <button
              key={url}
              type="button"
              className={`image-gallery__dot ${i === index ? "is-active" : ""}`}
              onClick={() => goTo(i)}
              aria-label={`Go to photo ${i + 1}`}
            />
          ))}
        </div>
      )}
    </div>
  );
}
