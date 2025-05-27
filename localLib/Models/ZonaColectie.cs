namespace localLib.Models
{
    public class ZonaColectie
    {
        public long ZonaColectieId { get; set; }
        public string DenumireZona { get; set; }
        public ICollection<Carte> Carti { get; set; }
    }
}
