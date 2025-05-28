using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using localLib.Models;

namespace localLib.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        [Display(Name = "Titlu")]
        public string Title { get; set; } = default!;

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author name cannot exceed 100 characters")]
        public string Author { get; set; } = default!;

        [StringLength(20)]
        public string ISBN { get; set; } = default!;

        [Required]
        [Range(1000, 2100, ErrorMessage = "Invalid publication year")]
        public int PublicationYear { get; set; } = default!;

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        public Genre Genre { get; set; } = default!;

        [Required]
        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }

        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; } = default!;

        [Range(1, 2000)]
        public int PageCount { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = default!;

        [DataType(DataType.ImageUrl)]
        public string CoverImageUrl { get; set; } = default!;

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
        public string ShelfLocation { get; set; } = default!;

        [Display(Name = "Is Featured?")]
        public bool IsFeatured { get; set; } = false;

        [Display(Name = "Edition")]
        public string Edition { get; set; } = default!;

        [Display(Name = "Language")]
        public string Language { get; set; } = "English";
    }
}