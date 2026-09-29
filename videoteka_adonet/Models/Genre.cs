namespace videoteka_adonet.Models
{
    // Žanr
    public class Genre
    {
        // Surogatni primarni ključ
        public int GenreId { get; set; }

        // Naziv žanra (hrvatski)
        public string Naziv { get; set; }

        // Alias za bindingi/ostatak UI koji koristi 'Name'
        public string Name
        {
            get => Naziv;
            set => Naziv = value;
        }
    }
}
