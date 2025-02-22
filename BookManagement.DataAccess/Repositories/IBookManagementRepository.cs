using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagement.Models.Dto;
using BookManagementAPI.Dto;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.DataAccess.Repositories
{
    public interface IBookManagementRepository
    {
         Task<bool> BookExists(int Id);
         Task<bool> BookExists(string Title);    
         Task<List<string>> GetBooksByPopularityScore(int PageNumber, int PageSize);
         Task<BookDetailsDto> GetBookDetails(int Id);
         Task<bool> AddBook(BookDto bookDto);
         Task<(List<BookDto> Addedbooks, List<BookDto> ExistBooks)> AddBooks(List<BookDto> bookDtos);
         Task<(bool, BookDto)> UpdateBook(int Id, BookDto bookDto);
         Task<(bool Result, BookDto bookdto)> DeleteBook(int Id);
        Task<(bool Result, List<BookDto> SoftDelated, List<int> CouldNotBeFound)> DeleteBooks(List<int> Ids);
        Task<List<Books>> GetToBeDeletedBooks(List<int> Ids);
    }
}
