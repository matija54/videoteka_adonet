using System;

namespace videoteka_adonet.Models
{
    // Posudba (loan)
    public class Posudba
    {
        public int FilmId { get; set; }
        public int KupacId { get; set; }

        public DateTime DatumPosudbe { get; set; }
        public DateTime? DatumVracanja { get; set; }

        // helper fields populated from queries
        public string FilmName { get; set; }
        public string CustomerName { get; set; }

        // English aliases used by some UI bindings
        public DateTime DatumPosudbe_dt { get => DatumPosudbe; set => DatumPosudbe = value; }
        public DateTime? DatumVracanja_dt { get => DatumVracanja; set => DatumVracanja = value; }
    }
}
