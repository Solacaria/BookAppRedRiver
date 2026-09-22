using BookApi.Models;
namespace BookApi.Services;


public interface IBookService
{
    Task<bool> DeleteBookAsync(int id);
    Task<List<Book>> GetBooksAsync(int? id, string? title, string? author);
    Task<Book?> UpdateBookAsync(Book book);
    Task<Book> CreateBookAsync(Book book);
    Task<List<Book>> GetAllBooksAsync();
}