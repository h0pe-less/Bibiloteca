namespace localLib.Models
{
    public class Editura
    {
        public long EdituraId { get; set; }
        public string Denumire { get; set; }
        public ICollection<Carte> Carti { get; set; }
    }
}
