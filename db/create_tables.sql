IF DB_ID(N'VideotekaDB') IS NULL
BEGIN
	CREATE DATABASE [VideotekaDB];
END
GO

USE [VideotekaDB];
GO

-- Table: Genre (Žanr)
CREATE TABLE Genre (
	GenreId INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- Table: Film
CREATE TABLE Film (
	FilmId INT IDENTITY(1,1) PRIMARY KEY,
	Code NVARCHAR(50) NOT NULL UNIQUE, -- šifra
	Name NVARCHAR(200) NOT NULL,       -- naziv
	GenreId INT NULL,                  -- žanr (nije obavezan)
	Description NVARCHAR(MAX) NULL,
	Quantity INT NOT NULL CONSTRAINT CK_Film_Quantity_NonNegative CHECK (Quantity >= 0),
	CONSTRAINT FK_Film_Genre FOREIGN KEY (GenreId) REFERENCES Genre(GenreId) ON DELETE SET NULL
);
GO

-- Table: Kupac (customer)
CREATE TABLE Kupac (
	KupacId INT IDENTITY(1,1) PRIMARY KEY,
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	Address NVARCHAR(300) NULL,
	MembershipNumber CHAR(8) NOT NULL UNIQUE,
	CONSTRAINT CK_Kupac_MembershipNumber_Format CHECK (LEN(MembershipNumber) = 8 AND MembershipNumber NOT LIKE '%[^0-9]%')
);
GO

-- Table: Posudba (loan) with composite primary key (FilmId, KupacId, DatumPosudbe)
CREATE TABLE Posudba (
	FilmId INT NOT NULL,
	KupacId INT NOT NULL,
	DatumPosudbe DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	DatumVracanja DATETIME2 NULL,
	PRIMARY KEY (FilmId, KupacId, DatumPosudbe),
	CONSTRAINT FK_Posudba_Film FOREIGN KEY (FilmId) REFERENCES Film(FilmId) ON DELETE NO ACTION,
	CONSTRAINT FK_Posudba_Kupac FOREIGN KEY (KupacId) REFERENCES Kupac(KupacId) ON DELETE CASCADE
);
GO

-- Index to help find outstanding loans quickly
CREATE INDEX IX_Posudba_Film_DatumVracanja ON Posudba(FilmId, DatumVracanja);
GO

-- NOTE: Application layer should enforce business rules such as "cannot issue a film when no copies are available".
-- A trigger could be added here to enforce availability at DB level if desired.
