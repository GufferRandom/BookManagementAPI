using System.ComponentModel.DataAnnotations;

namespace BookManagementAPI.Models
{
    public class Books
    {
        [Key]
        public int Id {get;set;}
        [Required]
        public string Title {get;set;}
        [Required]
        public int PublicationYear {get;set;}
        [Required]
        public string AuthorName {get;set;}
        public int ViewsCount {get;set;}=0;
        public bool SoftDeleted { get; set; }=false;
    }
}
