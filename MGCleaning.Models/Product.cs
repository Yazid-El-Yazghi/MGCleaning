namespace MGCleaning.Models;

/// <summary>
/// Product entiteit
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // bijv. "Vloerreiniger", "Toiletproduct"
    public decimal HoeveelheidPerM2 { get; set; }
    public decimal HoeveelheidPerToilet { get; set; }
    public int Voorraad { get; set; }
    public bool IsDeleted { get; set; }

    // Display property voor UI (x100)
    public decimal HoeveelheidPer100M2Display => HoeveelheidPerM2 * 100;

    // Navigatie eigenschappen
    public ICollection<Bestelling> Bestellingen { get; set; } = new List<Bestelling>();
}
