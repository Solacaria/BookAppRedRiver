using BookApi.Models;
namespace BookApi.Services;

public interface IQuoteService
{
    Task<bool> DeleteQuoteAsync(Quote quote);
    Task<List<Quote>> GetQuotesAsync(int? id, string? text, string? author, string year = "Unknown");
    Task<Quote?> UpdateQuoteAsync(Quote quote);
    Task<Quote> CreateQuoteAsync(Quote quote);
    Task<List<Quote>> GetAllQuotesAsync();
}