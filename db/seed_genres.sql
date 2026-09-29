USE [VideotekaDB];
GO

-- Seed a set of common genres (only insert when not present)
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Action') INSERT INTO Genre(Name) VALUES (N'Action');
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Comedy') INSERT INTO Genre(Name) VALUES (N'Comedy');
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Drama') INSERT INTO Genre(Name) VALUES (N'Drama');
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Horror') INSERT INTO Genre(Name) VALUES (N'Horror');
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Thriller') INSERT INTO Genre(Name) VALUES (N'Thriller');
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Science Fiction') INSERT INTO Genre(Name) VALUES (N'Science Fiction');
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Romance') INSERT INTO Genre(Name) VALUES (N'Romance');
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Documentary') INSERT INTO Genre(Name) VALUES (N'Documentary');
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Animation') INSERT INTO Genre(Name) VALUES (N'Animation');
IF NOT EXISTS (SELECT 1 FROM Genre WHERE Name = N'Adventure') INSERT INTO Genre(Name) VALUES (N'Adventure');

GO

-- Optional: show inserted genres
SELECT GenreId, Name FROM Genre ORDER BY Name;
