namespace localLib.Models
{
    public class CarteAutor
    {
        public long CarteId { get; set; }
        public Carte Carte { get; set; }

        public long AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
