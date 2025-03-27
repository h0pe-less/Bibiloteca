using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace localLib.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author name cannot exceed 100 characters")]
        public string Author { get; set; }

        [StringLength(20)]
        public string ISBN { get; set; }

        [Required]
        [Range(1000, 2100, ErrorMessage = "Invalid publication year")]
        public int PublicationYear { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        [StringLength(50)]
        public string Publisher { get; set; }

        [Range(1, 2000)]
        public int PageCount { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string CoverImageUrl { get; set; }

        [Required]
        [Display(Name = "Available Copies")]
        [Range(0, 1000, ErrorMessage = "Invalid quantity")]
        public int AvailableCopies { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "Shelf Location")]
        public string ShelfLocation { get; set; }

        [Display(Name = "Is Featured?")]
        public bool IsFeatured { get; set; } = false;

        [Display(Name = "Edition")]
        public string Edition { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; } = "English";
    }
}