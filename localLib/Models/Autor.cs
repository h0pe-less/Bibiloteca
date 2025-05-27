namespace localLib.Models
{
    public class Autor
    {
        public long AutorId { get; set; }
        public string NumePrenume { get; set; }
        public string Biografie { get; set; }
        public DateTime DataNasterii { get; set; }

        public ICollection<CarteAutor> CartiAutor { get; set; }
    }
}
