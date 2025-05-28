using System;
using System.ComponentModel.DataAnnotations;

namespace localLib.Models
{
    public class Imprumut
    {
        [Key]
        public long ImprumutId { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(100, ErrorMessage = "Numele nu poate depăși 100 caractere")]
        public string Nume { get; set; } = default!;

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        [StringLength(100, ErrorMessage = "Prenumele nu poate depăși 100 caractere")]
        public string Prenume { get; set; } = default!;

        [Required(ErrorMessage = "Grupa este obligatorie")]
        [StringLength(20, ErrorMessage = "Grupa nu poate depăși 20 caractere")]
        public string Grupa { get; set; } = default!;

        [Required(ErrorMessage = "Data împrumutului este obligatorie")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data împrumutului")]
        public DateTime DataImprumut { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data returnării")]
        public DateTime? DataReturnare { get; set; } = null;

        [Required(ErrorMessage = "Cartea este obligatorie")]
        [Display(Name = "Carte")]
        public long CarteId { get; set; }

        public Carte Carte { get; set; } = default!;

        [Display(Name = "Carte returnată")]
        public bool EsteReturnat { get; set; } = false;
    }
}