using MGCleaning.Models;
using Microsoft.EntityFrameworkCore;

namespace MGCleaning.Desktop.Services;

/// <summary>
/// Service voor arbeider operaties
/// </summary>
public class ArbeiderService
{
    private readonly ApplicationDbContext _context;

    public ArbeiderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Arbeider>> GetAlleArbeidersAsync()
    {
        try
        {
            return await _context.Arbeiders
                .Include(a => a.GebouwArbeiders)
                .ThenInclude(ga => ga.Gebouw)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ophalen arbeiders: {ex.Message}", ex);
        }
    }

    public async Task<Arbeider?> GetArbeiderByIdAsync(int id)
    {
        try
        {
            return await _context.Arbeiders
                .Include(a => a.GebouwArbeiders)
                .ThenInclude(ga => ga.Gebouw)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ophalen arbeider: {ex.Message}", ex);
        }
    }

    public async Task ToevoegenAsync(Arbeider arbeider)
    {
        try
        {
            _context.Arbeiders.Add(arbeider);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij toevoegen arbeider: {ex.Message}", ex);
        }
    }

    public async Task BijwerkenAsync(Arbeider arbeider)
    {
        try
        {
            _context.Arbeiders.Update(arbeider);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij bijwerken arbeider: {ex.Message}", ex);
        }
    }

    public async Task VerwijderenAsync(int id)
    {
        try
        {
            var arbeider = await _context.Arbeiders.FindAsync(id);
            if (arbeider != null)
            {
                arbeider.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij verwijderen arbeider: {ex.Message}", ex);
        }
    }

    public async Task KoppelAanGebouwAsync(int arbeiderId, int gebouwId)
    {
        try
        {
            var bestaatAl = await _context.GebouwArbeiders
                .AnyAsync(ga => ga.ArbeiderId == arbeiderId && ga.GebouwId == gebouwId);

            if (!bestaatAl)
            {
                _context.GebouwArbeiders.Add(new GebouwArbeider
                {
                    ArbeiderId = arbeiderId,
                    GebouwId = gebouwId
                });
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij koppelen arbeider aan gebouw: {ex.Message}", ex);
        }
    }

    public async Task OntkoppelVanGebouwAsync(int arbeiderId, int gebouwId)
    {
        try
        {
            var koppeling = await _context.GebouwArbeiders
                .FirstOrDefaultAsync(ga => ga.ArbeiderId == arbeiderId && ga.GebouwId == gebouwId);

            if (koppeling != null)
            {
                _context.GebouwArbeiders.Remove(koppeling);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ontkoppelen arbeider van gebouw: {ex.Message}", ex);
        }
    }
}
