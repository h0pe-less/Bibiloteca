namespace localLib.Models
{
    public class Limba
    {
        public long LimbaId { get; set; }
        public string DenumireLimba { get; set; }

        public ICollection<Carte> Carti { get; set; }
    }
}
