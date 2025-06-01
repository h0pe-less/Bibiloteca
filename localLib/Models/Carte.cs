using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace localLib.Models
{
    public class Carte
    {
        public long CarteId { get; set; }
        public string? ISBN { get; set; }
        public string? ISSN { get; set; }
        public string Cota { get; set; } = default!;
        public string Titlu { get; set; } = default!;

        [DisplayName("Titlu Info")]
        public string TitluInfo { get; set; } = default!;
        public string MentiuniResponsabilitate { get; set; } = default!;
        public string Editie { get; set; } = default!;
        public long EdituraId { get; set; }
        public Editura? Editura { get; set; }

        [DisplayName("Data publicării")]
        public DateTime DataPublicarii { get; set; }

        [DisplayName("Locul publicării")]
        public string LoculPublicarii { get; set; } = default!;
        public string Bibliografie { get; set; } = default!;
        public string Descriere { get; set; } = default!;

        [DisplayName("Număr de pagini")]
        public int NrPagini { get; set; }
        public decimal Pret { get; set; }
        public long ZonaColectieId { get; set; }

        [DisplayName("Zona colecție")]
        public ZonaColectie? ZonaColectie { get; set; }
        public long LimbaId { get; set; }
        public Limba? Limba { get; set; }
        public long TaraId { get; set; }
        public Tara? Tara { get; set; }

        [DisplayName("Număr inventar")]
        public long NumarInventar { get; set; }
        public string Paginatie { get; set; } = default!;
        public string Ilustratii { get; set; } = default!;
        public string? CopertaURL { get; set; }
        public ICollection<CarteAutor> Autori { get; set; } = new HashSet<CarteAutor>();
        public ICollection<CarteCategorie> CarteCategorii { get; set; } = new HashSet<CarteCategorie>();
    }
}
