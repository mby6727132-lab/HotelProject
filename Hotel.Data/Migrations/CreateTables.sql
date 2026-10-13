-- =====================================================================
-- HotelProjectDb - Table creation script
-- Generated to match Hotel.Core Entities + Hotel.Data Fluent API Configurations
-- Equivalent to what `dotnet ef migrations add Init` would produce for SQL Server
-- =====================================================================

IF DB_ID(N'HotelProjectDb') IS NULL
BEGIN
    CREATE DATABASE HotelProjectDb;
END
GO

USE HotelProjectDb;
GO

-- ---------------------------------------------------------------------
-- Hotels
-- ---------------------------------------------------------------------
CREATE TABLE dbo.Hotels
(
    Id      INT IDENTITY(1,1) NOT NULL,
    Name    NVARCHAR(200)     NOT NULL,
    City    NVARCHAR(100)     NOT NULL,
    Address NVARCHAR(300)     NOT NULL,
    CONSTRAINT PK_Hotels PRIMARY KEY (Id)
);
GO

CREATE INDEX IX_Hotels_City ON dbo.Hotels (City);
GO

-- ---------------------------------------------------------------------
-- Users
-- ---------------------------------------------------------------------
CREATE TABLE dbo.Users
(
    Id           INT IDENTITY(1,1) NOT NULL,
    Name         NVARCHAR(200)     NOT NULL,
    Email        NVARCHAR(256)     NOT NULL,
    PasswordHash NVARCHAR(MAX)     NOT NULL,
    Role         NVARCHAR(50)      NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY (Id)
);
GO

CREATE UNIQUE INDEX IX_Users_Email ON dbo.Users (Email);
GO

-- ---------------------------------------------------------------------
-- Rooms (1:N from Hotels, RowVersion for Optimistic Concurrency)
-- ---------------------------------------------------------------------
CREATE TABLE dbo.Rooms
(
    Id             INT IDENTITY(1,1) NOT NULL,
    RoomNumber     INT               NOT NULL,
    Floor          INT               NOT NULL,
    Price          DECIMAL(10,2)     NOT NULL,
    HotelId        INT               NOT NULL,
    LastBookingAt  DATETIME2         NULL,
    RowVersion     ROWVERSION        NOT NULL,
    CONSTRAINT PK_Rooms PRIMARY KEY (Id),
    CONSTRAINT FK_Rooms_Hotels_HotelId FOREIGN KEY (HotelId)
        REFERENCES dbo.Hotels (Id) ON DELETE CASCADE
);
GO

CREATE INDEX IX_Rooms_HotelId ON dbo.Rooms (HotelId);
GO

-- ---------------------------------------------------------------------
-- Payments (N:1 to Users, N:1 to Rooms)
-- ---------------------------------------------------------------------
CREATE TABLE dbo.Payments
(
    Id       INT IDENTITY(1,1) NOT NULL,
    UserId   INT               NOT NULL,
    RoomId   INT               NOT NULL,
    CheckIn  DATETIME2         NOT NULL,
    CheckOut DATETIME2         NOT NULL,
    Amount   DECIMAL(10,2)     NOT NULL,
    IsPaid   BIT               NOT NULL DEFAULT (0),
    CONSTRAINT PK_Payments PRIMARY KEY (Id),
    CONSTRAINT FK_Payments_Users_UserId FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Payments_Rooms_RoomId FOREIGN KEY (RoomId)
        REFERENCES dbo.Rooms (Id) ON DELETE NO ACTION
);
GO

CREATE INDEX IX_Payments_UserId ON dbo.Payments (UserId);
CREATE INDEX IX_Payments_RoomId ON dbo.Payments (RoomId);
GO

-- ---------------------------------------------------------------------
-- UserFavoriteHotels (N:M join table between Users and Hotels)
-- ---------------------------------------------------------------------
CREATE TABLE dbo.UserFavoriteHotels
(
    UserId  INT NOT NULL,
    HotelId INT NOT NULL,
    CONSTRAINT PK_UserFavoriteHotels PRIMARY KEY (UserId, HotelId),
    CONSTRAINT FK_UserFavoriteHotels_Users_UserId FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserFavoriteHotels_Hotels_HotelId FOREIGN KEY (HotelId)
        REFERENCES dbo.Hotels (Id) ON DELETE CASCADE
);
GO

CREATE INDEX IX_UserFavoriteHotels_HotelId ON dbo.UserFavoriteHotels (HotelId);
GO
