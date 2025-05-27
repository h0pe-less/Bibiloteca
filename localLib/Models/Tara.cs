namespace localLib.Models
{
    public class Tara
    {
        public long TaraId { get; set; }
        public string DenumireTara { get; set; }

        public ICollection<Carte> Carti { get; set; }
    }
}
