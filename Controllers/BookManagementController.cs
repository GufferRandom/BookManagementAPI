using System.ComponentModel.DataAnnotations;
using BookManagementAPI.Data;
using BookManagementAPI.Dto;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookManagementController:ControllerBase
    {
        private readonly AppDataContext _context;
        public BookManagementController(AppDataContext context)
        {
            _context = context;
        }
        [HttpGet("GetBooksByPopularityScore")]
        public IActionResult GetBooksByPopularityScore([FromQuery,Range(1,99999)] int PageSize = 5, [FromQuery,Range(1,99999)] int PageNumber=1)
        {
            var books =_context.Books.OrderByDescending(x => (int)Math.Round(x.ViewsCount * 0.5)
            -((DateTime.Now.Year-x.PublicationYear)*2)).Skip((PageNumber-1)*PageSize).Take(PageSize)
            .Select(x=>x.Title).ToList();
            return Ok(books);
        }
        [HttpGet("GetBook/{Id:int}")]
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
        [HttpPost("AddBook")]
        public IActionResult AddBook([FromBody] List<BookDto> booksdto)
        {
            if(booksdto.Count==0)
            {
                return BadRequest("Cannot Add zero books");
            }
            List<BookDto> found=booksdto.Where(x=>_context.Books
            .Select(y=>y.Title).Contains(x.Title)).ToList();
            var foundtitles = found.Select(x => x.Title).ToList();
            if (found.Count == booksdto.Count)
            {
                var replyB = new
                {
                    Messege= "Book Exists Or All Books Arleady Exist ",
                    AlreadyExist = foundtitles
                };
                return BadRequest(replyB);
            }
           var result = booksdto.Except(found).ToList();
           List<Books> books=result.Select(x=>new Books {
           Title = x.Title, AuthorName = x.AuthorName, PublicationYear=x.PublicationYear }).ToList();
           _context.Books.AddRange(books);
           _context.SaveChanges();
            var reply = new
            {
               Added = result,
               AlreadyExist = foundtitles
            };
            return Ok(reply);
        }
        [HttpPut("UpdateBook/{Id:int}")]
        public IActionResult UpdateBook(int Id, [FromBody] BookDto bookdto)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == Id);
            if (book == null)
            {
                var response = new
                {
                    Messege = "The Book Cant Be Found. Please Enter Valid Book Id",
                    BookID = Id
                };
                return NotFound(response);
            }
            if (_context.Books.Any(x => x.Title == bookdto.Title))
            {
                var response = new
                {
                    Messege = "The Book Arleady Exists .Enter Diffrent Title",
                    Book = bookdto
                };
                return BadRequest(response);
            }
            var OldTitle= book.Title;
            var OldAuthorName = book.AuthorName;
            var OldPublicationYear = book.PublicationYear;
            book.Title = bookdto.Title;
            book.AuthorName = bookdto.AuthorName;
            book.PublicationYear = bookdto.PublicationYear;
            _context.SaveChanges();
            var reply = new
            {
                Messege = "Book Updated Successfully",
                beforeUpdateBook = new BookDto { Title =OldTitle, AuthorName =OldAuthorName, PublicationYear = OldPublicationYear},
                AfterUpdateBook = bookdto
            };
            return Ok(reply);
        }
        [HttpDelete("DeleteBooks")]
        public IActionResult DeleteBooks([FromBody] List<int> BookIds)
        {
            if (BookIds.Count == 0)
            {
                return BadRequest("Cannot Delete zero books");
            }
            var books = _context.Books.Where(x => x.SoftDeleted == false && BookIds.Contains(x.Id)).ToList();
            List<int> ErrorBookIds=new();
            if (books.Count!=BookIds.Count)
            {
                ErrorBookIds = BookIds.Except(books.Select(x => x.Id)).ToList();
            }
            if (books.Count == 0)
            {
                var response = new
                {
                    Messege = "The Book Or The Books Cant Be Found.Enter Valid BookID",
                    ErrorBookIds
                };
                return BadRequest(response);
            }
            foreach(var i in books)
            {
                i.SoftDeleted= true;
            }
            List<int> SoftDeletedIds = new();
            if (ErrorBookIds.Count > 0)
            {
                SoftDeletedIds = BookIds.Except(ErrorBookIds).ToList();
            }
            var res = new
            {
                SoftDeleted = SoftDeletedIds,
                ErrorBookIds
            };
            _context.SaveChanges();
            return Ok(res);
        }
    }
}
