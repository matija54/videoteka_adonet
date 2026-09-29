namespace videoteka_adonet.Models
{
    // Film entity
    public class Film
    {
        public int FilmId { get; set; }

        // šifra
        public string Sifra { get; set; }

        // naziv
        public string Naziv { get; set; }

        // žanr (može biti null)
        public int? GenreId { get; set; }

        // prikaz naziva žanra (helper polje popunjeno iz DB-a)
        public string GenreName { get; set; }

        // opis
        public string Opis { get; set; }

        // količina
        public int Kolicina { get; set; }

        // English aliases used by the simple UI bindings added earlier
        public string Code { get => Sifra; set => Sifra = value; }
        public string Name { get => Naziv; set => Naziv = value; }
        public int Quantity { get => Kolicina; set => Kolicina = value; }
    }
}
