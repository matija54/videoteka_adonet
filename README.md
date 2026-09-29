# Videoteka - upute za pokretanje

Kratko: jednostavna WPF (.NET 10) aplikacija koja koristi ADO.NET i SQL Server LocalDB. Projekt je student‑style (minimalan code‑behind, bez EF).

Preduvjeti
- Visual Studio 2022/2026 ili drugi editor s podrškom za .NET 10
- .NET 10 SDK
- SQL Server Express LocalDB (instaliran kao (localdb)\\MSSQLLocalDB)

Postupak za postavljanje baze
1. Provjerite da je LocalDB instanca dostupna: u PowerShellu pokrenite `SqlLocalDB info MSSQLLocalDB`.
2. Otvorite SQL Server Management Studio ili koristite `sqlcmd`.
3. U workspace\db\ nalazi se:
   - create_tables.sql  (kreira bazu VideotekaDB i tablice)
   - seed_genres.sql    (ubacuje ~10 žanrova)
   - seed_sample_data.sql (neobavezno, ubacuje par filmova, kupaca i posudbu za test)
4. Pokrenite skripte redom: create_tables.sql, zatim seed_genres.sql, zatim seed_sample_data.sql.
   Primjer preko sqlcmd (PowerShell):
   sqlcmd -S "(localdb)\\MSSQLLocalDB" -i "C:\\Users\\matij\\source\\repos\\videoteka_adonet\\db\\create_tables.sql"
   Ponovite za seed_genres.sql i seed_sample_data.sql.

Podešavanje connection stringa
- Projekat koristi App.config (connectionStrings) s imenom `VideotekaDB`.
- Zadana vrijednost: `Server=(localdb)\\MSSQLLocalDB;Database=VideotekaDB;Trusted_Connection=True;`.
- Ako koristite drugu instancu ili server, prilagodite connection string u videoteka_adonet\App.config.

Pokretanje aplikacije
1. Otvorite videoteka_adonet.slnx u Visual Studio.
2. Build -> Rebuild Solution.
3. Start (F5) — pojavi se glavni prozor s izbornikom.
4. Kroz izbornik otvarajte:
   - Administracija filmova
   - Administracija kupaca
   - Posudbe
   - Izvještaji (Stanje na skladištu, Kartica kupca)

Bilješke o funkcionalnostima
- Brisanje kupca će obrisati njegove posudbe (ON DELETE CASCADE u bazi).
- Prilikom izdavanja posudbe provjerava se raspoloživost (ne dozvoljava se izdavanje ako nema primjeraka).
- Jednostavne validacije su u UI (obavezna polja, format članskog broja, nenegativna količina).
- Kod je svjestan toga da treba izgledati "studentski" (minimalan, bez kompleksnih arhitektura).

Gdje se nalaze bitne datoteke
- videoteka_adonet/: WPF projekt (XAML i code-behind)
- videoteka_adonet/App.config: connection string
- videoteka_adonet/Data/Database.cs: ADO.NET helper
- db/: SQL skripte (create, seed)

Ako naiđete na problem pri izvršavanju skripti javite točno koju grešku dobijete i ja ću pomoći.
