using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookManagement.Models.Dto;
using BookManagementAPI.Data;
using BookManagementAPI.Dto;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccess.Repositories
{
    public class BookManagementRepository:IBookManagementRepository
    {
        private readonly AppDataContext _context;
        public BookManagementRepository(AppDataContext context)
        {
            _context = context;
        }
        public async Task<List<string>> GetBooksByPopularityScore(int PageNumber,int PageSize)
        {
            var books = await _context.Books.Where(x => x.SoftDeleted == false)
                .OrderByDescending(x => (int)Math.Round(x.ViewsCount * 0.5)
           - ((DateTime.Now.Year - x.PublicationYear) * 2)).Skip((PageNumber - 1) * PageSize).Take(PageSize)
           .Select(x => x.Title).ToListAsync();
            return books;
        }
        public async Task<BookDetailsDto> GetBookDetails(int Id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == Id && x.SoftDeleted == false);
            if (book == null)
            {
                return null;
            }
            book.ViewsCount++;
            await _context.SaveChangesAsync();
            int YearsSincePublished = DateTime.Now.Year - book.PublicationYear;
            int PopularityScore = (int)Math.Round(book.ViewsCount * 0.5) - (YearsSincePublished * 2);
            BookDetailsDto bookDetailsDto = new BookDetailsDto()
            {
                Title = book.Title,
                AuthorName = book.AuthorName,
                PublicationYear = book.PublicationYear,
                ViewsCount = book.ViewsCount,
                YearsSincePublished = YearsSincePublished,
                PopularityScore = PopularityScore

            };
            return bookDetailsDto;
        }
        public async Task<bool> AddBook(BookDto bookDto)
        {
            Books book  = new Books(){
                Title=bookDto.Title,
                AuthorName=bookDto.AuthorName,
                PublicationYear = bookDto.PublicationYear
            };
            try
            {
                await _context.AddAsync(book);
                await _context.SaveChangesAsync();
             return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<(List<BookDto> Addedbooks,List<BookDto> ExistBooks)> AddBooks(List<BookDto> bookDtos)
        {
          
            var bookTitles = await _context.Books
            .Where(y => y.SoftDeleted == false)
            .Select(y => y.Title.ToLower())
            .ToListAsync();
            var found = bookDtos.Where(x => bookTitles.Contains(x.Title.ToLower())).ToList();
            var AddedBooks=bookDtos.Except(found).ToList();
            var res = AddedBooks.Select(x=>new Books
            {
                Title = x.Title,
                AuthorName = x.AuthorName,
                PublicationYear = x.PublicationYear
            }).ToList();
            try
            {
            await _context.AddRangeAsync(res);
            await _context.SaveChangesAsync();
            return (AddedBooks,found);
            }
            catch(Exception ex)
            {
                return (null,null);
            }
        }
        public async Task<(bool,BookDto)> UpdateBook(int Id, BookDto bookDto)
        {
            var book =await _context.Books.FirstOrDefaultAsync(x => x.Id == Id);
            string OldTitle= book.Title;
            string OldAuthorName = book.AuthorName;
            int OldPublicationYear = book.PublicationYear;

            book.Title=bookDto.Title;
            book.AuthorName = bookDto.AuthorName;
            book.PublicationYear = bookDto.PublicationYear;
            try
            {
                await _context.SaveChangesAsync();
                return (true, new BookDto { Title = OldTitle, AuthorName = OldAuthorName, PublicationYear = OldPublicationYear });
            }
            catch (Exception ex)
            {
                return (false,null);
            }
        }
        public async Task<bool> BookExists(int Id)
        {

            bool exists = await _context.Books.AnyAsync(x => x.Id == Id && x.SoftDeleted ==false);
            return exists;
        }
        public async Task<bool> BookExists(string Title)
        {

            bool exists= await _context.Books.AnyAsync(x=>x.Title.ToLower()== Title.ToLower() && x.SoftDeleted == false);
            return exists;
        }
        public async Task<(bool Result,BookDto bookdto)> DeleteBook(int Id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == Id);
            try
            {
            book.SoftDeleted = true;
            await _context.SaveChangesAsync();
            return (true,new BookDto {Title=book.Title,AuthorName=book.AuthorName,PublicationYear=book.PublicationYear});
            }
            catch (Exception ex)
            {
                return (false,null);
            }
        }
        public async Task<(bool Result, List<BookDto> SoftDelated,List<int> CouldNotBeFound)> DeleteBooks(List<int> Ids)
        {
            var books = await GetToBeDeletedBooks(Ids);
            var booksdto = books.Select(x => new BookDto { AuthorName = x.AuthorName,
                PublicationYear = x.PublicationYear, Title = x.Title }).ToList();
            List<int> CouldNotBeFound = Ids.Except(books.Select(x => x.Id)).ToList();
            if (CouldNotBeFound.Count == Ids.Count)
            {
                return (false,booksdto, CouldNotBeFound);
            }
            try
            {
            foreach (var book in books)
            {
                book.SoftDeleted = true;
            }
                await _context.SaveChangesAsync();
                return (true, booksdto, CouldNotBeFound);
            }
            catch(Exception ex)
            {
                return (false, null, null);
            }
        }
        public async Task<List<Books>> GetToBeDeletedBooks(List<int> Ids)
        {
            var books= await _context
                .Books.Where(x => Ids.Contains(x.Id) && x.SoftDeleted == false).ToListAsync();
            return books;
        }
    }
}
