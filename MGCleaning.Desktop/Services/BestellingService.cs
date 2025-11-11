using MGCleaning.Models;
using Microsoft.EntityFrameworkCore;

namespace MGCleaning.Desktop.Services;

/// <summary>
/// Service voor bestelling operaties
/// </summary>
public class BestellingService
{
    private readonly ApplicationDbContext _context;

    public BestellingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Bestelling>> GetAlleBestellingenAsync()
    {
        try
        {
            return await _context.Bestellingen
                .Include(b => b.Gebouw)
                .Include(b => b.Product)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ophalen bestellingen: {ex.Message}", ex);
        }
    }

    public async Task<Bestelling?> GetBestellingByIdAsync(int id)
    {
        try
        {
            return await _context.Bestellingen
                .Include(b => b.Gebouw)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ophalen bestelling: {ex.Message}", ex);
        }
    }

    public async Task ToevoegenAsync(Bestelling bestelling)
    {
        try
        {
            _context.Bestellingen.Add(bestelling);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij toevoegen bestelling: {ex.Message}", ex);
        }
    }

    public async Task BijwerkenAsync(Bestelling bestelling)
    {
        try
        {
            _context.Bestellingen.Update(bestelling);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij bijwerken bestelling: {ex.Message}", ex);
        }
    }

    public async Task VerwijderenAsync(int id)
    {
        try
        {
            var bestelling = await _context.Bestellingen.FindAsync(id);
            if (bestelling != null)
            {
                bestelling.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij verwijderen bestelling: {ex.Message}", ex);
        }
    }
}
