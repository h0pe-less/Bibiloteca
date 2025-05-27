namespace localLib.Models
{
    public class Carte
    {
        public long CarteId { get; set; }
        public string ISBN { get; set; }
        public string ISSN { get; set; }
        public string Cota { get; set; }
        public string Titlu { get; set; }
        public string TitluInfo { get; set; }
        public string MentiuniResponsabilitate { get; set; }
        public string Editie { get; set; }

        public long EdituraId { get; set; }
        public Editura Editura { get; set; }

        public DateTime DataPublicarii { get; set; }
        public string LoculPublicarii { get; set; }
        public string Bibliografie { get; set; }
        public string Descriere { get; set; }
        public int NrPagini { get; set; }
        public decimal Pret { get; set; }

        public long ZonaColectieId { get; set; }
        public ZonaColectie ZonaColectie { get; set; }

        public long LimbaId { get; set; }
        public Limba Limba { get; set; }

        public long TaraId { get; set; }
        public Tara Tara { get; set; }

        public long NumarInventar { get; set; }


        public string Paginatie { get; set; }
        public string Ilustratii { get; set; }


        public string CopertaURL { get; set; }

        public ICollection<CarteAutor> Autori { get; set; }
        public ICollection<CarteCategorie> CarteCategorii { get; set; }
    }
}
