-- =====================================================================
-- HotelProjectDb - Seed data script
-- Run AFTER CreateTables.sql, on an empty database.
-- Matches spec section 9 (Seed Data): 5-8 hotels in a few cities,
-- 5-10 rooms per hotel, 1 Admin + at least 2 Customers,
-- several past/future bookings, several Favorite (N:M) relations.
--
-- NOTE on PasswordHash: AuthService (real BCrypt hashing) does not exist
-- yet in this stage, so every user below gets the SAME placeholder hash
-- value, clearly marked below. Once AuthService.RegisterAsync is built,
-- re-seed real users through the API instead of trusting this value.
-- =====================================================================

USE HotelProjectDb;
GO

-- ---------------------------------------------------------------------
-- Hotels (6 hotels across 4 cities)
-- ---------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Hotels ON;

INSERT INTO dbo.Hotels (Id, Name, City, Address) VALUES
(1, N'Dan Tel Aviv',      N'Tel Aviv', N'99 HaYarkon St.'),
(2, N'Sea View Suites',   N'Tel Aviv', N'45 HaYarkon St.'),
(3, N'King David',        N'Jerusalem', N'23 King David St.'),
(4, N'Mamilla Palace',    N'Jerusalem', N'12 Shlomo HaMelech St.'),
(5, N'Carmel View',       N'Haifa', N'5 Yefe Nof St.'),
(6, N'Herods Eilat',      N'Eilat', N'10 North Beach');

SET IDENTITY_INSERT dbo.Hotels OFF;
GO

-- ---------------------------------------------------------------------
-- Users (1 Admin + 3 Customers)
-- ---------------------------------------------------------------------
DECLARE @PlaceholderHash NVARCHAR(MAX) = N'PLACEHOLDER_HASH_$2a$11$demoOnlyNotARealBcryptHash';

SET IDENTITY_INSERT dbo.Users ON;

INSERT INTO dbo.Users (Id, Name, Email, PasswordHash, Role) VALUES
(1, N'Admin User',    N'admin@hotelproject.com', @PlaceholderHash, N'Admin'),
(2, N'Yossi Cohen',   N'yossi@example.com',      @PlaceholderHash, N'Customer'),
(3, N'Dana Levi',     N'dana@example.com',       @PlaceholderHash, N'Customer'),
(4, N'Noa Ben-David', N'noa@example.com',        @PlaceholderHash, N'Customer');

SET IDENTITY_INSERT dbo.Users OFF;
GO

-- ---------------------------------------------------------------------
-- Rooms (5 rooms per hotel = 30 rooms)
-- ---------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Rooms ON;

INSERT INTO dbo.Rooms (Id, RoomNumber, Floor, Price, HotelId, LastBookingAt) VALUES
-- Hotel 1: Dan Tel Aviv
(1,  101, 1, 450.00, 1, NULL),
(2,  102, 1, 450.00, 1, NULL),
(3,  201, 2, 620.00, 1, NULL),
(4,  202, 2, 620.00, 1, NULL),
(5,  301, 3, 950.00, 1, NULL),
-- Hotel 2: Sea View Suites
(6,  101, 1, 380.00, 2, NULL),
(7,  102, 1, 380.00, 2, NULL),
(8,  201, 2, 520.00, 2, NULL),
(9,  202, 2, 520.00, 2, NULL),
(10, 301, 3, 780.00, 2, NULL),
-- Hotel 3: King David
(11, 101, 1, 700.00, 3, NULL),
(12, 102, 1, 700.00, 3, NULL),
(13, 201, 2, 900.00, 3, NULL),
(14, 202, 2, 900.00, 3, NULL),
(15, 301, 3, 1450.00, 3, NULL),
-- Hotel 4: Mamilla Palace
(16, 101, 1, 650.00, 4, NULL),
(17, 102, 1, 650.00, 4, NULL),
(18, 201, 2, 890.00, 4, NULL),
(19, 202, 2, 890.00, 4, NULL),
(20, 301, 3, 1320.00, 4, NULL),
-- Hotel 5: Carmel View
(21, 101, 1, 320.00, 5, NULL),
(22, 102, 1, 320.00, 5, NULL),
(23, 201, 2, 410.00, 5, NULL),
(24, 202, 2, 410.00, 5, NULL),
(25, 301, 3, 560.00, 5, NULL),
-- Hotel 6: Herods Eilat
(26, 101, 1, 540.00, 6, NULL),
(27, 102, 1, 540.00, 6, NULL),
(28, 201, 2, 690.00, 6, NULL),
(29, 202, 2, 690.00, 6, NULL),
(30, 301, 3, 1100.00, 6, NULL);

SET IDENTITY_INSERT dbo.Rooms OFF;
GO

-- ---------------------------------------------------------------------
-- Payments (mix of past and future bookings, for availability testing)
-- ---------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Payments ON;

INSERT INTO dbo.Payments (Id, UserId, RoomId, CheckIn, CheckOut, Amount, IsPaid) VALUES
-- Past bookings
(1, 2, 1,  '2026-06-10', '2026-06-14', 1800.00, 1),   -- Room 1 (4 nights x 450)
(2, 3, 11, '2026-07-01', '2026-07-03', 1400.00, 1),   -- Room 11 (2 nights x 700)
(3, 4, 21, '2026-08-05', '2026-08-08',  960.00, 1),   -- Room 21 (3 nights x 320)
-- Future bookings (for testing the overlap / availability query)
(4, 2, 6,  '2026-11-01', '2026-11-05', 1520.00, 1),   -- Room 6  (4 nights x 380)
(5, 3, 16, '2026-12-20', '2026-12-27', 4550.00, 0),   -- Room 16 (7 nights x 650), not yet paid
(6, 4, 26, '2027-01-10', '2027-01-12', 1080.00, 1),   -- Room 26 (2 nights x 540)
(7, 2, 1,  '2026-12-01', '2026-12-03',  900.00, 1);   -- Room 1 again, different (non-overlapping) dates

SET IDENTITY_INSERT dbo.Payments OFF;
GO

-- Reflect the last booking on each booked room (as PaymentService will do going forward)
UPDATE dbo.Rooms SET LastBookingAt = '2026-12-03' WHERE Id = 1;
UPDATE dbo.Rooms SET LastBookingAt = '2026-07-01' WHERE Id = 11;
UPDATE dbo.Rooms SET LastBookingAt = '2026-08-05' WHERE Id = 21;
UPDATE dbo.Rooms SET LastBookingAt = '2026-11-01' WHERE Id = 6;
UPDATE dbo.Rooms SET LastBookingAt = '2026-12-20' WHERE Id = 16;
UPDATE dbo.Rooms SET LastBookingAt = '2027-01-10' WHERE Id = 26;
GO

-- ---------------------------------------------------------------------
-- UserFavoriteHotels (N:M favorites)
-- ---------------------------------------------------------------------
INSERT INTO dbo.UserFavoriteHotels (UserId, HotelId) VALUES
(2, 1),
(2, 3),
(3, 2),
(4, 5),
(4, 1);
GO
