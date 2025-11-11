namespace MGCleaning.Models;

/// <summary>
/// Bestelling entiteit
/// </summary>
public class Bestelling
{
    public int Id { get; set; }
    public int GebouwId { get; set; }
    public Gebouw Gebouw { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int FrequentiePerMaand { get; set; }
    public DateTime Datum { get; set; }
    public string? Bericht { get; set; }
    public bool IsDeleted { get; set; }
}
