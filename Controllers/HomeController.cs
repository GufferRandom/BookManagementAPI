using System.ComponentModel.DataAnnotations;
using BookManagementAPI.Data;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers
{
    public class HomeController:ControllerBase
    {
        private readonly AppDataContext _context;
        public HomeController(AppDataContext context)
        {
            _context = context;
        }
        [HttpGet("/GetBooksByPopularityScore")]
        public IActionResult GetBooksByPopularityScore([FromQuery,Range(1,99999)] int PageSize = 5, [FromQuery,Range(1,99999)] int PageNumber=1)
        {
            var books =_context.Books.OrderByDescending(x => (int)Math.Round(x.ViewsCount * 0.5)
            -((DateTime.Now.Year-x.PublicationYear)*2)).Skip((PageNumber-1)*PageSize).Take(PageSize)
            .Select(x=>x.Title).ToList();
            return Ok(books);
        }
        [HttpGet("/GetBook/{Id:int}")]
        public IActionResult GetBookDetails(int Id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == Id);
            if(book==null)
            {
                return NotFound("The Book Cant Be Found. Please Enter Valid Book Id");
            }
            book.ViewsCount++;
            _context.SaveChanges();
            int YearsSincePublished = DateTime.Now.Year-book.PublicationYear;
            int PopularityScore =(int)Math.Round(book.ViewsCount * 0.5)-(YearsSincePublished * 2);
            object bookextend = new { book.Title,
                book.AuthorName,
                book.PublicationYear,
                book.ViewsCount,
                YearsSincePublished,
                PopularityScore
            };
            return Ok(bookextend);
        }
    }
}
