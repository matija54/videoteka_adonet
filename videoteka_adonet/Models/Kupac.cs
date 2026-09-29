namespace videoteka_adonet.Models
{
    // Kupac (customer)
    public class Kupac
    {
        public int KupacId { get; set; }

        // ime
        public string Ime { get; set; }

        // prezime
        public string Prezime { get; set; }

        // adresa (nije obavezno)
        public string Adresa { get; set; }

        // broj članske iskaznice (osmeroznamenkasti string)
        public string ClanskiBroj { get; set; }

        // English aliases for UI compatibility
        public string FirstName { get => Ime; set => Ime = value; }
        public string LastName { get => Prezime; set => Prezime = value; }
        public string MembershipNumber { get => ClanskiBroj; set => ClanskiBroj = value; }
    }
}
