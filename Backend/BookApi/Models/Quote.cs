
namespace BookApi.Models;


public class Quote
{
    public int Id { get; set;}
    public required string Text { get; set; }
    public required string Author { get; set; }
    public string Year { get; set; } = "Unknown";
}