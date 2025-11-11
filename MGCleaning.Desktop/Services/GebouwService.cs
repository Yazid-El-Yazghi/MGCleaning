using MGCleaning.Models;
using Microsoft.EntityFrameworkCore;

namespace MGCleaning.Desktop.Services;

/// <summary>
/// Service voor gebouw operaties
/// </summary>
public class GebouwService
{
    private readonly ApplicationDbContext _context;

    public GebouwService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Gebouw>> GetAlleGebouwenAsync()
    {
        try
        {
            // Lambda expressie
            return await _context.Gebouwen
                .Include(g => g.GebouwArbeiders)
                .ThenInclude(ga => ga.Arbeider)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ophalen gebouwen: {ex.Message}", ex);
        }
    }

    public async Task<Gebouw?> GetGebouwByIdAsync(int id)
    {
        try
        {
            return await _context.Gebouwen
                .Include(g => g.GebouwArbeiders)
                .ThenInclude(ga => ga.Arbeider)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ophalen gebouw: {ex.Message}", ex);
        }
    }

    public async Task ToevoegenAsync(Gebouw gebouw)
    {
        try
        {
            _context.Gebouwen.Add(gebouw);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij toevoegen gebouw: {ex.Message}", ex);
        }
    }

    public async Task BijwerkenAsync(Gebouw gebouw)
    {
        try
        {
            _context.Gebouwen.Update(gebouw);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij bijwerken gebouw: {ex.Message}", ex);
        }
    }

    public async Task VerwijderenAsync(int id)
    {
        try
        {
            var gebouw = await _context.Gebouwen.FindAsync(id);
            if (gebouw != null)
            {
                gebouw.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij verwijderen gebouw: {ex.Message}", ex);
        }
    }

    // Query syntax voorbeeld
    public async Task<List<Gebouw>> ZoekGebouwenAsync(string zoekterm)
    {
        try
        {
            var query = from g in _context.Gebouwen
                        where g.Naam.Contains(zoekterm) || g.Adres.Contains(zoekterm)
                        orderby g.Naam
                        select g;
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij zoeken gebouwen: {ex.Message}", ex);
        }
    }
}
