namespace MGCleaning.Models;

/// <summary>
/// Gebouw entiteit
/// </summary>
public class Gebouw
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;
    public decimal OppervlakteM2 { get; set; }
    public int AantalToiletten { get; set; }
    public bool IsDeleted { get; set; }

    // Navigatie eigenschappen
    public ICollection<GebouwArbeider> GebouwArbeiders { get; set; } = new List<GebouwArbeider>();
    public ICollection<Bestelling> Bestellingen { get; set; } = new List<Bestelling>();
}
