using MGCleaning.Models;
using Microsoft.EntityFrameworkCore;

namespace MGCleaning.Desktop.Services;

/// <summary>
/// Service voor product operaties
/// </summary>
public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAlleProductenAsync()
    {
        try
        {
            return await _context.Producten.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ophalen producten: {ex.Message}", ex);
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            return await _context.Producten.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ophalen product: {ex.Message}", ex);
        }
    }

    public async Task ToevoegenAsync(Product product)
    {
        try
        {
            _context.Producten.Add(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij toevoegen product: {ex.Message}", ex);
        }
    }

    public async Task BijwerkenAsync(Product product)
    {
        try
        {
            _context.Producten.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij bijwerken product: {ex.Message}", ex);
        }
    }

    public async Task VerwijderenAsync(int id)
    {
        try
        {
            var product = await _context.Producten.FindAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij verwijderen product: {ex.Message}", ex);
        }
    }
}
