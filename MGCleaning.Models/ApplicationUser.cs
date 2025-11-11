using Microsoft.AspNetCore.Identity;

namespace MGCleaning.Models;

/// <summary>
/// Uitgebreide Identity gebruiker met extra velden
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? Telefoonnummer { get; set; }
    public string? Adres { get; set; }
}
