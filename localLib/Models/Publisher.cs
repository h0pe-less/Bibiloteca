using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace localLib.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Publisher name is required")]
        [StringLength(100, ErrorMessage = "Publisher name cannot exceed 100 characters")]
        public string Name { get; set; } = default!;

        [StringLength(200)]
        public string Address { get; set; } = default!;

        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = default!;

        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;

        [StringLength(200)]
        [DataType(DataType.Url)]
        public string Website { get; set; } = default!;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}