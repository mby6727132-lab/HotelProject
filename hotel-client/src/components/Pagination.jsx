import "./Pagination.css";

// Generic client-side pager for an endpoint that doesn't return a total count.
// "Next" is enabled whenever the current page came back full (a strong signal
// more results may exist); "Previous" is disabled on page 1.
export default function Pagination({ page, pageSize, resultCount, onPageChange }) {
  const hasPrevious = page > 1;
  const hasNext = resultCount === pageSize;

  if (!hasPrevious && !hasNext) return null;

  return (
    <div className="pagination">
      <button
        type="button"
        className="btn btn-outline btn-sm"
        onClick={() => onPageChange(page - 1)}
        disabled={!hasPrevious}
      >
        ← Previous
      </button>
      <span className="pagination__label">Page {page}</span>
      <button
        type="button"
        className="btn btn-outline btn-sm"
        onClick={() => onPageChange(page + 1)}
        disabled={!hasNext}
      >
        Next →
      </button>
    </div>
  );
}
