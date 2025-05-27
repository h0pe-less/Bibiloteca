namespace localLib.Models
{
    public class Categorie
    {
        public long CategorieId { get; set; }
        public string Denumire { get; set; }
        public string Descriere { get; set; }

        public ICollection<CarteCategorie> CartiCategorii { get; set; }
    }
}
