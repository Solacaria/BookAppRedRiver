using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuoteController(IQuoteService quoteService) : ControllerBase
{
    //Ej implementerad i frontend.
    [HttpGet("read")]
    public async Task<ActionResult<List<Quote>>> GetQuotes(string year, int? id = null, string? text = null, string? author = null)
    {
        if (id == null && text == null && author == null && year == "Unknown")
        {
            return BadRequest("Minst en parameter måste anges: id, text eller bookId.");
        }

        try
        {
            var quotes = await quoteService.GetQuotesAsync(id, text, author, year);
            if (quotes.Count == 0) return NotFound(new
            {
                message = "Hittade inga citat med de angivna parametrarna."
            });
            return Ok(quotes);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Misslyckades med hämning av citat." });
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateQuote([FromBody] Quote quote)
    {
        if (quote.Text == null || quote.Author == null)
        {
            return BadRequest("Alla parametrar måste anges: Citat, källa.");
        }

        try
        {
            var newQuote = await quoteService.CreateQuoteAsync(quote);
            return Ok(newQuote);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Misslyckades med skapande av citat." });
        }
    }

    [HttpPut("edit")]
    public async Task<IActionResult> UpdateQuote([FromBody] Quote quote)
    {
        if (quote.Text == null && quote.Author == null && quote.Year == null)
        {
            return BadRequest("Minst en parameter måste anges: text, author eller year.");  
        }

        try
        {
            var newQuote = await quoteService.UpdateQuoteAsync(quote);
            if (newQuote == null) return NotFound(new
            {
                message = "Inget citat hittades med dom angivna parametrarna."
            });
            return Ok(newQuote);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Misslyckades med uppdatering av citat." });
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteQuote([FromBody] Quote quote)
    {
        try
        {
            var sucess = await quoteService.DeleteQuoteAsync(quote);
            if (!sucess) return NotFound(new
            {
                message = "Inget citat hittades, borttagning misslyckades."
            });
            return Ok(new { message = "Citatet togs bort." });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Misslyckades med borttagning av citat." });
        }
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<List<Quote>>> GetAllQuotes()
    {
        try
        {
            return await quoteService.GetAllQuotesAsync();
        }
        catch (Exception)
        {
            return BadRequest(new {message = "Fanns inga citat att hämta."});
        }
        
    }


}