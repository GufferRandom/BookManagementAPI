using System.ComponentModel.DataAnnotations;

namespace BookManagementAPI.Dto
{
    public class BookDto
    {
        [Required, MinLength(2), MaxLength(1000)]
        public string Title { get; set; } = "";
        [Required, MinLength(2), MaxLength(1000)]
        public string AuthorName { get; set; } = "";
        [Required, Range(0, 2025)]
        public int PublicationYear { get; set; }
    }
}
