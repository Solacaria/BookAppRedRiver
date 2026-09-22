using System.Diagnostics;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService) : ControllerBase
{    
    //Ej implementerad i frontend. 
    [HttpGet("read")]
    public async Task<ActionResult<List<Book>>> GetBooks(int? id = null, string? title = null, string? author = null)
    {
        if (id == null && title == null && author == null)
        {
            return BadRequest("Minst en parameter måste anges: id, titel eller författare.");
        }

        try
        {
            var books = await bookService.GetBooksAsync(id, title, author);
            if (books.Count == 0) return NotFound(new
            {
                message = "Hittade inga böcker med de angivna parametrarna."
            });
            return Ok(books);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBook([FromBody] Book book)
    {        
        if (book.Title == null || book.Author == null || book.PublishedDate == null)
        {
            return BadRequest("Alla parametrar måste fyllas i");
        }
        try
        {
            var newBook = await bookService.CreateBookAsync(book);
            return Ok(newBook);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPut("edit")]
    public async Task<IActionResult> UpdateBook([FromBody] Book book)
    {
        if (book.Title == null && book.Author == null && book.PublishedDate == null)
        {
            return BadRequest("Minst en parametrar måste anges: titel, författare eller publiceringsdatum.");
        }
        try
        {
            var updatedBook = await bookService.UpdateBookAsync(book);

            if (updatedBook == null) return NotFound(new
            {
                message = "Ingen bok hittades med dom angivna parametrarna."
            });
            return Ok(updatedBook);
        }
        catch (Exception)
        {
           return BadRequest();
        }
        
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteBook([FromBody] Book book)
    {
        try
        {
            var success = await bookService.DeleteBookAsync(book.Id);
            if (!success) return NotFound(new 
            { 
                message = "Ingen bok hittades, och kunde inte tas bort." 
            });
            return Ok( new { message = "Boken togs bort." });
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<List<Book>>> GetAllBooks()
    {
        try
        {
            return await bookService.GetAllBooksAsync();
        }
        catch (Exception)
        {
            return BadRequest(new {message = "Fanns inga böcker att hämta."});
        }
        
    }
}