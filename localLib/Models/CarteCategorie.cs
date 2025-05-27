namespace localLib.Models
{
    public class CarteCategorie
    {
        public long CarteId { get; set; }
        public Carte Carte { get; set; }

        public long CategorieId { get; set; }
        public Categorie Categorie { get; set; }
    }
}
