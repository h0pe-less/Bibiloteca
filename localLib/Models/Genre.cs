using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace localLib.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Genre name is required")]
        [StringLength(50, ErrorMessage = "Genre name cannot exceed 50 characters")]
        public string Name { get; set; } = default!;

        [StringLength(200)]
        public string Description { get; set; } = default!;
        
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}