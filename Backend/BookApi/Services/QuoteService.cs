using BookApi.Models;
using BookApi.Database;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services;

public class QuoteService : IQuoteService
{
    private readonly AppDbContext _dbContext;
    public QuoteService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Quote> CreateQuoteAsync(Quote quote)
    {
       var newQuote = new Quote
        {
            Text = quote.Text,
            Author = quote.Author,
            Year = quote.Year
        };

        await _dbContext.Quotes.AddAsync(newQuote);
        await _dbContext.SaveChangesAsync();

        return quote;
    }

    public async Task<bool> DeleteQuoteAsync(Quote quote)
    {
        var newQuote = await _dbContext.Quotes.FirstOrDefaultAsync(q => q.Id == quote.Id);
        if (newQuote == null) return false;

        _dbContext.Quotes.Remove(newQuote);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Quote>> GetQuotesAsync(int? id, string? text, string? author, string year = "Unknown")
    {
        var query = _dbContext.Quotes.AsQueryable();

        if (id.HasValue) query = query.Where(q => q.Id == id.Value);
        if (!string.IsNullOrWhiteSpace(text)) query = query.Where(q => q.Text.Contains(text));
        if (!string.IsNullOrWhiteSpace(author)) query = query.Where(q => q.Author.Contains(author));
        if (!string.IsNullOrWhiteSpace(year) && year != "Unknown") query = query.Where(q => q.Year == year);

        return await query.ToListAsync();
    }

    public async Task<Quote?> UpdateQuoteAsync(Quote quote)
    {
        var updatedQuote = await _dbContext.Quotes.FirstOrDefaultAsync(q => q.Id == quote.Id);

        if (updatedQuote == null) return null;

        updatedQuote.Text = quote.Text;
        updatedQuote.Author = quote.Author;
        updatedQuote.Year = quote.Year;

        await _dbContext.SaveChangesAsync();
        return updatedQuote;
    
    }

    public async Task<List<Quote>> GetAllQuotesAsync()
    {
        return _dbContext.Quotes.ToList();
    }


}