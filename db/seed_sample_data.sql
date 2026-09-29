USE [VideotekaDB];
GO

-- Seed sample films (ensure genres are present)
-- This script uses simple assumptions about Genre names inserted by seed_genres.sql

DECLARE @Action INT = (SELECT GenreId FROM Genre WHERE Name = N'Action');
DECLARE @Comedy INT = (SELECT GenreId FROM Genre WHERE Name = N'Comedy');
DECLARE @Drama INT = (SELECT GenreId FROM Genre WHERE Name = N'Drama');

IF NOT EXISTS (SELECT 1 FROM Film WHERE Code = N'F001')
	INSERT INTO Film(Code, Name, GenreId, Description, Quantity) VALUES (N'F001', N'The Great Adventure', @Action, N'Avanturistički film', 3);
IF NOT EXISTS (SELECT 1 FROM Film WHERE Code = N'F002')
	INSERT INTO Film(Code, Name, GenreId, Description, Quantity) VALUES (N'F002', N'Funny Moments', @Comedy, N'Komedija', 5);
IF NOT EXISTS (SELECT 1 FROM Film WHERE Code = N'F003')
	INSERT INTO Film(Code, Name, GenreId, Description, Quantity) VALUES (N'F003', N'Deep Drama', @Drama, N'Drama', 2);

-- Seed some customers
IF NOT EXISTS (SELECT 1 FROM Kupac WHERE MembershipNumber = '00000001')
	INSERT INTO Kupac(FirstName, LastName, Address, MembershipNumber) VALUES (N'Ivan', N'Ivić', N'Ulica 1', '00000001');
IF NOT EXISTS (SELECT 1 FROM Kupac WHERE MembershipNumber = '00000002')
	INSERT INTO Kupac(FirstName, LastName, Address, MembershipNumber) VALUES (N'Marka', N'Marković', N'Ulica 2', '00000002');

-- Seed a posudba
DECLARE @FilmId1 INT = (SELECT TOP 1 FilmId FROM Film WHERE Code = N'F001');
DECLARE @Kupac1 INT = (SELECT TOP 1 KupacId FROM Kupac WHERE MembershipNumber = '00000001');
IF NOT EXISTS (SELECT 1 FROM Posudba WHERE FilmId = @FilmId1 AND KupacId = @Kupac1)
	INSERT INTO Posudba(FilmId, KupacId, DatumPosudbe) VALUES (@FilmId1, @Kupac1, DATEADD(day, -3, SYSUTCDATETIME()));

GO

SELECT 'Seed finished' AS Info;
