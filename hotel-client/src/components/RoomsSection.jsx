import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getAvailableRooms } from "../api/roomsApi";
import { getErrorMessage } from "../api/axiosClient";
import { useSearch } from "../context/SearchContext";
import RoomCard from "./RoomCard";
import Pagination from "./Pagination";
import SkeletonCard from "./SkeletonCard";
import EmptyState from "./EmptyState";
import Alert from "./Alert";
import "./RoomsSection.css";

const PAGE_SIZE = 6;

// The existing room search / availability / selection mechanism, unchanged -
// just extracted so it can live inline on the hotel page instead of its own route.
export default function RoomsSection({ hotel }) {
  const navigate = useNavigate();
  const { criteria, setCriteria } = useSearch();

  const [dates, setDates] = useState({ checkIn: criteria.checkIn, checkOut: criteria.checkOut });
  const [guests, setGuests] = useState(criteria.guests);
  const [page, setPage] = useState(1);
  const [rooms, setRooms] = useState([]);
  const [status, setStatus] = useState("idle"); // idle | loading | loaded | error
  const [errorMessage, setErrorMessage] = useState("");
  const [dateError, setDateError] = useState("");

  const fetchRooms = useCallback(
    async (targetPage) => {
      if (new Date(dates.checkOut) <= new Date(dates.checkIn)) {
        setDateError("Check-out must be after check-in.");
        return;
      }
      setDateError("");
      setStatus("loading");
      setErrorMessage("");
      try {
        const results = await getAvailableRooms(hotel.id, {
          checkIn: dates.checkIn,
          checkOut: dates.checkOut,
          guests,
          page: targetPage,
          pageSize: PAGE_SIZE,
        });
        setRooms(results);
        setPage(targetPage);
        setStatus("loaded");
      } catch (err) {
        setErrorMessage(getErrorMessage(err, "Could not load available rooms."));
        setStatus("error");
      }
    },
    [hotel.id, dates.checkIn, dates.checkOut, guests]
  );

  useEffect(() => {
    fetchRooms(1);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [hotel.id]);

  function handleSearchSubmit(e) {
    e.preventDefault();
    setCriteria({ checkIn: dates.checkIn, checkOut: dates.checkOut, guests });
    fetchRooms(1);
  }

  function handleSelectRoom(room) {
    setCriteria({ checkIn: dates.checkIn, checkOut: dates.checkOut, guests });
    navigate("/booking", { state: { hotel, room, checkIn: dates.checkIn, checkOut: dates.checkOut, guests } });
  }

  return (
    <div className="rooms-section">
      <form className="date-form card" onSubmit={handleSearchSubmit}>
        <div className="field">
          <label htmlFor="check-in">Check-in</label>
          <input
            id="check-in"
            type="date"
            value={dates.checkIn}
            onChange={(e) => setDates({ ...dates, checkIn: e.target.value })}
          />
        </div>
        <div className="field">
          <label htmlFor="check-out">Check-out</label>
          <input
            id="check-out"
            type="date"
            value={dates.checkOut}
            onChange={(e) => setDates({ ...dates, checkOut: e.target.value })}
          />
        </div>
        <div className="field">
          <label htmlFor="guests">Guests</label>
          <input
            id="guests"
            type="number"
            min={1}
            max={10}
            value={guests}
            onChange={(e) => setGuests(Number(e.target.value))}
          />
        </div>
        <button type="submit" className="btn btn-primary" disabled={status === "loading"}>
          {status === "loading" ? "Searching..." : "Search Availability"}
        </button>
      </form>

      {dateError && <Alert type="error">{dateError}</Alert>}
      {status === "error" && <Alert type="error">{errorMessage}</Alert>}

      {status === "loading" && <SkeletonCard count={3} variant="room" />}

      {status === "loaded" && rooms.length === 0 && (
        <EmptyState
          icon="🛏"
          title="No rooms available for these dates"
          description={`Try a different date range, or reduce the number of guests (currently ${guests}).`}
        />
      )}

      {status === "loaded" && rooms.length > 0 && (
        <>
          <div className="room-grid">
            {rooms.map((room) => (
              <RoomCard key={room.id} room={room} onSelect={handleSelectRoom} />
            ))}
          </div>
          <Pagination page={page} pageSize={PAGE_SIZE} resultCount={rooms.length} onPageChange={fetchRooms} />
        </>
      )}
    </div>
  );
}
