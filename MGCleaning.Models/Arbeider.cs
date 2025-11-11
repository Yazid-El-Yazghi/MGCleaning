namespace MGCleaning.Models;

/// <summary>
/// Arbeider entiteit
/// </summary>
public class Arbeider
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public bool IsVerantwoordelijke { get; set; }
    public string Telefoonnummer { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    // Navigatie eigenschappen
    public ICollection<GebouwArbeider> GebouwArbeiders { get; set; } = new List<GebouwArbeider>();
}
