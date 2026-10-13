import { useEffect, useState } from "react";
import { getHotelsByCity } from "../api/hotelsApi";
import { getAvailableRooms } from "../api/roomsApi";
import { getErrorMessage } from "../api/axiosClient";
import { useSearch } from "../context/SearchContext";
import HotelCard from "../components/HotelCard";
import DealCard from "../components/DealCard";
import SkeletonCard from "../components/SkeletonCard";
import EmptyState from "../components/EmptyState";
import Alert from "../components/Alert";
import "./HotelsPage.css";

const POPULAR_CITIES = ["Tel Aviv", "Jerusalem", "Haifa"];

export default function HotelsPage() {
  const { criteria, setCriteria } = useSearch();
  const [form, setForm] = useState(criteria);
  const [hotels, setHotels] = useState([]);
  const [status, setStatus] = useState("idle"); // idle | loading | loaded | error
  const [errorMessage, setErrorMessage] = useState("");
  const [searchedCity, setSearchedCity] = useState("");
  const [deals, setDeals] = useState([]);
  const [dealsLoading, setDealsLoading] = useState(true);

  // "Special Offers" band: real hotels from a few popular cities, each with a
  // real per-night price for 2 guests looked up from the rooms endpoint (no
  // fabricated discount numbers).
  useEffect(() => {
    Promise.all(POPULAR_CITIES.map((city) => getHotelsByCity(city).catch(() => [])))
      .then(async (results) => {
        const topHotels = results.flat().slice(0, 4);
        const withPrices = await Promise.all(
          topHotels.map(async (hotel) => {
            try {
              const rooms = await getAvailableRooms(hotel.id, {
                checkIn: criteria.checkIn,
                checkOut: criteria.checkOut,
                guests: 2,
                page: 1,
                pageSize: 50,
              });
              const cheapest = rooms.length ? Math.min(...rooms.map((r) => r.price)) : null;
              return { ...hotel, priceForCouple: cheapest };
            } catch {
              return { ...hotel, priceForCouple: null };
            }
          })
        );
        setDeals(withPrices);
      })
      .finally(() => setDealsLoading(false));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  async function runSearch(searchForm) {
    const city = searchForm.city.trim();
    if (!city) {
      setErrorMessage("Please enter a city to search.");
      setStatus("error");
      return;
    }

    setStatus("loading");
    setErrorMessage("");
    try {
      const results = await getHotelsByCity(city, searchForm.destination.trim());
      setHotels(results);
      setSearchedCity(city);
      setCriteria(searchForm);
      setStatus("loaded");
    } catch (err) {
      setErrorMessage(getErrorMessage(err, "Could not load hotels for that city."));
      setStatus("error");
    }
  }

  function handleSubmit(e) {
    e.preventDefault();
    runSearch(form);
  }

  function handleQuickCity(city) {
    const nextForm = { ...form, city };
    setForm(nextForm);
    runSearch(nextForm);
  }

  return (
    <div className="page">
      {/* 1. Header / introductory section */}
      <header className="page-header">
        <div className="container">
          <span className="page-header__eyebrow">Find your perfect stay</span>
          <h1>Discover Exceptional Hotels, Wherever You're Headed</h1>
        </div>
      </header>

      {/* 2. Search and filter bar - directly visible, no scrolling required */}
      <section className="search-section">
        <div className="container">
          <form className="search-form" onSubmit={handleSubmit}>
            <div className="search-form__row">
              <div className="search-form__field">
                <label htmlFor="destination">Hotel Name / Destination</label>
                <input
                  id="destination"
                  type="text"
                  placeholder="e.g. Dan, King David..."
                  value={form.destination}
                  onChange={(e) => setForm({ ...form, destination: e.target.value })}
                />
              </div>
              <div className="search-form__field">
                <label htmlFor="city-search">City</label>
                <input
                  id="city-search"
                  type="text"
                  placeholder="e.g. Tel Aviv"
                  value={form.city}
                  onChange={(e) => setForm({ ...form, city: e.target.value })}
                />
              </div>
            </div>
            <div className="search-form__row">
              <div className="search-form__field">
                <label htmlFor="checkIn">Check-in</label>
                <input
                  id="checkIn"
                  type="date"
                  value={form.checkIn}
                  onChange={(e) => setForm({ ...form, checkIn: e.target.value })}
                />
              </div>
              <div className="search-form__field">
                <label htmlFor="checkOut">Check-out</label>
                <input
                  id="checkOut"
                  type="date"
                  value={form.checkOut}
                  onChange={(e) => setForm({ ...form, checkOut: e.target.value })}
                />
              </div>
              <div className="search-form__field search-form__field--narrow">
                <label htmlFor="guests">Guests</label>
                <input
                  id="guests"
                  type="number"
                  min={1}
                  max={10}
                  value={form.guests}
                  onChange={(e) => setForm({ ...form, guests: Number(e.target.value) })}
                />
              </div>
              <button type="submit" className="btn btn-primary search-form__submit" disabled={status === "loading"}>
                {status === "loading" ? "Searching..." : "Search Hotels"}
              </button>
            </div>
          </form>

          <div className="hero__quick-cities">
            <span>Popular:</span>
            {POPULAR_CITIES.map((c) => (
              <button key={c} type="button" className="hero__quick-city" onClick={() => handleQuickCity(c)}>
                {c}
              </button>
            ))}
          </div>
        </div>
      </section>

      {(dealsLoading || deals.length > 0) && (
        <section className="deals-band">
          <div className="container">
            <h2 className="deals-band__title">✨ Special Offers &amp; Featured Stays</h2>
            {dealsLoading ? (
              <div className="deals-band__scroller">
                {[1, 2, 3].map((i) => (
                  <div key={i} className="deals-band__skeleton skeleton-shimmer" />
                ))}
              </div>
            ) : (
              <div className="deals-band__scroller">
                {deals.map((hotel) => (
                  <DealCard key={hotel.id} hotel={hotel} />
                ))}
              </div>
            )}
          </div>
        </section>
      )}

      {/* 3. List of hotels matching the search criteria */}
      <section className="section">
        <div className="container">
          {status === "loading" && <SkeletonCard count={3} />}

          {status === "error" && <Alert type="error">{errorMessage}</Alert>}

          {status === "loaded" && hotels.length === 0 && (
            <EmptyState
              icon="🏨"
              title={`No hotels found in "${searchedCity}"`}
              description="Try searching for a different city, such as Tel Aviv or Jerusalem."
            />
          )}

          {status === "loaded" && hotels.length > 0 && (
            <>
              <h2 className="results-heading">
                {hotels.length} {hotels.length === 1 ? "hotel" : "hotels"} in {searchedCity}
              </h2>
              <div className="hotel-grid">
                {hotels.map((hotel) => (
                  <HotelCard key={hotel.id} hotel={hotel} />
                ))}
              </div>
            </>
          )}

          {status === "idle" && (
            <EmptyState
              icon="🔍"
              title="Search for a city to get started"
              description="Try one of the popular cities above, or type your own."
            />
          )}
        </div>
      </section>
    </div>
  );
}
