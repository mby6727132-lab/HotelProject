import { createContext, useContext, useMemo, useState } from "react";

const SearchContext = createContext(null);

function defaultDates() {
  const checkIn = new Date();
  checkIn.setDate(checkIn.getDate() + 1);
  const checkOut = new Date();
  checkOut.setDate(checkOut.getDate() + 2);
  const toInputDate = (d) => d.toISOString().slice(0, 10);
  return { checkIn: toInputDate(checkIn), checkOut: toInputDate(checkOut) };
}

const INITIAL_CRITERIA = {
  destination: "",
  city: "",
  ...defaultDates(),
  guests: 2,
};

// Holds the user's search criteria (destination/city, dates, guests) so it
// persists across Hotels -> Hotel Detail -> Rooms -> Booking without re-entry.
export function SearchProvider({ children }) {
  const [criteria, setCriteriaState] = useState(INITIAL_CRITERIA);

  const setCriteria = (patch) => setCriteriaState((prev) => ({ ...prev, ...patch }));

  const value = useMemo(() => ({ criteria, setCriteria }), [criteria]);

  return <SearchContext.Provider value={value}>{children}</SearchContext.Provider>;
}

export function useSearch() {
  const ctx = useContext(SearchContext);
  if (!ctx) throw new Error("useSearch must be used within a SearchProvider");
  return ctx;
}
