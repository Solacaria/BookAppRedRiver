using BookApi.Models;
using BookApi.Database;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services;

public class BookService : IBookService
{

    private readonly AppDbContext _dbContext;

    public BookService(AppDbContext dbcontext)
    {
        _dbContext = dbcontext;
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        var newBook = new Book
        {
            Title = book.Title,
            Author = book.Author,
            PublishedDate = book.PublishedDate
        };

        await _dbContext.Books.AddAsync(newBook);
        await _dbContext.SaveChangesAsync();

        return book;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book == null) return false;

        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Book?> UpdateBookAsync(Book book)
    {
        var updatedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);

        if (updatedBook == null) return null;
        updatedBook.Title = book.Title;
        updatedBook.Author = book.Author;
        updatedBook.PublishedDate = book.PublishedDate;
        
        await _dbContext.SaveChangesAsync();
        return updatedBook;
    }

    public async Task<List<Book>> GetBooksAsync(int? id, string? title, string? author)
    {
        var query = _dbContext.Books.AsQueryable();

        if (id.HasValue) query = query.Where(b => b.Id == id.Value);
        if (!string.IsNullOrWhiteSpace(title)) query = query.Where(b => b.Title.Contains(title));
        if (!string.IsNullOrWhiteSpace(author)) query = query.Where(b => b.Author.Contains(author));

        return await query.ToListAsync();
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return _dbContext.Books.ToList();
    }
}