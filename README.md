# Hotel Booking API

ASP.NET Core Web API (.NET 8) for a hotel room booking system, built in four layers
(`Hotel.Core`, `Hotel.Data`, `Hotel.Service`, `Hotel.API`) over SQL Server with EF Core,
JWT authentication, and centralized middleware/logging.

## The constrained resource

The constrained resource is **a specific room for a specific date range**. Two booking
requests can race to reserve the same room for overlapping dates, and at most one must
succeed.

`PaymentService.CreateAsync` first runs a business-rule pre-check (does an overlapping
`Payment` already exist for that room?) — this catches the common case but, on its own,
has a race window between the check and the save. The actual safety net is **Optimistic
Concurrency**: `Room.RowVersion` ( SQL Server `rowversion`, configured via `[Timestamp]`
and `IsRowVersion()`) is included by EF Core in the `WHERE` clause of every `UPDATE` to
that row. When two requests load the same room and both try to save, the first `SaveChangesAsync`
succeeds and bumps the row's version; the second's `UPDATE` then matches zero rows, and EF Core
throws `DbUpdateConcurrencyException`. `PaymentService` catches that specific exception and
turns it into `ConcurrencyConflictException`, which `GlobalExceptionMiddleware` maps to an
HTTP **409 Conflict** with a clear message (and logs it at **Warning** level, with the
request's Correlation ID). The pre-check overlap case (`RoomUnavailableException`) is handled
the same way, also as a 409.

This is proven by an integration test (`Hotel.Tests/Concurrency/ConcurrencyTests.cs`) that
opens two separate `AppDbContext` instances against the real database, has both load the same
room, and both attempt a conflicting save — the first succeeds, the second throws
`DbUpdateConcurrencyException`.

## Running locally

**Database required:** SQL Server (LocalDB is sufficient — `(localdb)\mssqllocaldb`).

1. Restore and build:
   ```
   dotnet build
   ```
2. Configure secrets (never committed — `appsettings.json` holds no connection string or JWT key):
   ```
   cd Hotel.API
   dotnet user-secrets set "ConnectionStrings:Default" "Server=(localdb)\mssqllocaldb;Database=HotelProjectDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   dotnet user-secrets set "Jwt:Key" "<a random string, at least 32 bytes>"
   ```
3. Apply migrations (run from the solution root):
   ```
   dotnet ef database update -p Hotel.Data -s Hotel.API
   ```
4. Run the API:
   ```
   dotnet run --project Hotel.API
   ```
   Swagger UI is available at `/swagger` in Development, with a "Bearer" auth button for
   testing `[Authorize]`-protected endpoints with a JWT.

No manual database setup is needed beyond step 3 — migrations create the schema **and**
seed demo data (hotels, rooms, users, sample bookings, favorites).

## Demo users (seeded)

| Role     | Email                       | Password     |
|----------|------------------------------|---------------|
| Admin    | admin@hotelproject.com      | `Passw0rd!`   |
| Customer | customer@hotelproject.com   | `Passw0rd!`   |
| Customer | customer2@hotelproject.com  | `Passw0rd!`   |

Admins can manage hotels/rooms (`POST`/`PUT`/`DELETE`); Customers can browse, favorite
hotels, and book rooms. Both roles can book/favorite; only Admin can manage inventory.

## Migrations

Two migrations exist in `Hotel.Data/Migrations`:
- `InitialCreate` — initial schema (Hotels, Rooms, Users, Payments, UserFavoriteHotels).
- `SeedInitialData` — seed data for all of the above (`HasData`).

## Tests

`Hotel.Tests` (xUnit + Moq) covers `HotelService`, `RoomService`, `PaymentService`, and
`AuthService` business logic against mocked repositories, plus the real two-`DbContext`
concurrency integration test described above. Run with:
```
dotnet test
```
