using System.ComponentModel.DataAnnotations;
using BookManagement.DataAccess.Repositories;
using BookManagementAPI.Data;
using BookManagementAPI.Dto;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace BookManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookManagementController:ControllerBase
    {
        private readonly AppDataContext _context;
        private readonly IBookManagementRepository _bookManagementRepository;
        public BookManagementController(AppDataContext context, IBookManagementRepository bookManagementRepository)
        {
            _context = context;
            _bookManagementRepository = bookManagementRepository;
        }
        [HttpGet("GetBooksByPopularityScore")]
        public async Task<IActionResult> GetBooksByPopularityScore([FromQuery,Range(1,99999)] int PageSize = 5, [FromQuery,Range(1,99999)] int PageNumber=1)
        {
            var books= await _bookManagementRepository.GetBooksByPopularityScore(PageNumber, PageSize);
            return Ok(books);
        }
        [HttpGet("GetBook/{Id:int}")]
        public async Task<IActionResult> GetBookDetails(int Id)
        {
            if (await _bookManagementRepository.BookExists(Id)==false)
            {
                var response = new
                {
                    Messege = "The Book Cant Be Found. Please Enter Valid Book Id",
                    BookID = Id
                };
                return NotFound(response);
            }
            var book =await _bookManagementRepository.GetBookDetails(Id);
            return Ok(book);
        }
        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto){
            bool AddedBook = await _bookManagementRepository.AddBook(bookDto);
            if (!AddedBook){
                var res = new
                {
                    Messege = "The Book Arleady Exists.Enter diffrent Title",
                    Book = bookDto
                };
                return BadRequest(res);
            }
            var response= new
            {
                Messege = "The Book Was Added Successfully",
                Book = bookDto
            };
            return Ok(response);
        }
        [HttpPost("AddBooks")]
        public async Task<IActionResult> AddBooks([FromBody] List<BookDto> booksdto)
        {
           var (books,ArleadyExists)= await _bookManagementRepository.AddBooks(booksdto);
            if (booksdto.Count == 0)
            {
                return BadRequest("Cannot Add Zero Books");
            }
            if(books==null)
            {
                return StatusCode(500,"Server Error Could not save to database");
            }
            if (ArleadyExists.Count == booksdto.Count)
            {
                var resp = new
                {
                    Messege = "The Books/Book Arleady Exist/Exists",
                    ArleadyExists
                };
                return BadRequest(resp);
            }
            var response = new
            {
                Messege = "The Books Were Added Successfully",
                AddedBooks= books,
                ArleadyExists
            };
            return Ok(response);
        }
        [HttpPut("UpdateBook/{Id:int}")]
        public async Task<IActionResult> UpdateBook(int Id, [FromBody] BookDto bookdto)
        {
            if (await _bookManagementRepository.BookExists(Id)== false)
            {
                var response = new
                {
                    Messege = "The Book Cant Be Found. Please Enter Valid Book Id",
                    BookID = Id
                };
                return NotFound(response);
            }
            if (await _bookManagementRepository.BookExists(bookdto.Title))
            {
                var response = new
                {
                    Messege = "The Book Arleady Exists .Enter Diffrent Title",
                    Book = bookdto
                };
                return BadRequest(response);
            }
            (bool UpdatedBook,BookDto beforeUpdateBook) = await _bookManagementRepository.UpdateBook(Id, bookdto);
            if(!UpdatedBook)
            {
                return StatusCode(500, "Server Error Could not update the book in database");
            }
            var reply = new
            {
                Messege = "Book Updated Successfully",
                beforeUpdateBook,
                AfterUpdateBook = bookdto
            };
            return Ok(reply);
        }
        [HttpDelete("DeleteBook/{Id:int}")]
        public async Task<IActionResult> DeleteBook(int Id){
            if (await _bookManagementRepository.BookExists(Id) == false)
            {
                return NotFound("The BookId Cannot Be Found Or Was Deleted.Enter diffrent  Book id");
            }
            (bool Result,BookDto bookdto)= await _bookManagementRepository.DeleteBook(Id);
            if(!Result)
            {
                return StatusCode(500, "Server Error Could not delete the book in database");
            }
            var response = new{
                Messege="The Book Was Deleted Succesfully",
                DeletedBook=new BookDto{Title= bookdto.Title,AuthorName= bookdto.AuthorName
                ,PublicationYear= bookdto.PublicationYear}
            };
            return Ok(response);
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
                return NotFound(response);
            }
            foreach(var i in books)
            {
                i.SoftDeleted= true;
            }
            List<int> SoftDeletedIds = new();
            SoftDeletedIds = BookIds.Except(ErrorBookIds).ToList();
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
