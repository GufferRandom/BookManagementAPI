using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Models.Dto
{
    public class BookDetailsDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public int PublicationYear { get; set; }
        public int ViewsCount { get; set; } = 0;
        public int YearsSincePublished { get; set; } = 0;
        public int PopularityScore { get; set; } = 0;
    }
}
