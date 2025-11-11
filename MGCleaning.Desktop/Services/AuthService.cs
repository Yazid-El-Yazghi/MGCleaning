using MGCleaning.Models;
using Microsoft.AspNetCore.Identity;

namespace MGCleaning.Desktop.Services;

/// <summary>
/// Service voor authenticatie en gebruikersbeheer
/// </summary>
public class AuthService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUser? HuidigeGebruiker { get; private set; }

    public AuthService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> LoginAsync(string usernameOrEmail, string wachtwoord)
    {
        try
        {
            // Probeer eerst als username
            var user = await _userManager.FindByNameAsync(usernameOrEmail);
            
            // Als niet gevonden, probeer als email
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            }
            
            if (user == null)
                return false;

            // Check of gebruiker geblokkeerd is
            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new Exception("Dit account is geblokkeerd. Neem contact op met de beheerder.");
            }

            var result = await _userManager.CheckPasswordAsync(user, wachtwoord);
            if (result)
            {
                HuidigeGebruiker = user;
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij inloggen: {ex.Message}", ex);
        }
    }

    public async Task<bool> RegisterAsync(string username, string wachtwoord, string email, string telefoonnummer, string adres, string rol)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                Telefoonnummer = telefoonnummer,
                Adres = adres
            };

            var result = await _userManager.CreateAsync(user, wachtwoord);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, rol);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij registreren: {ex.Message}", ex);
        }
    }

    public Task LogoutAsync()
    {
        try
        {
            HuidigeGebruiker = null;
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij uitloggen: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> GetRollenAsync(ApplicationUser user)
    {
        try
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij ophalen rollen: {ex.Message}", ex);
        }
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
    {
        try
        {
            return await _userManager.IsInRoleAsync(user, role);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij controleren rol: {ex.Message}", ex);
        }
    }
}
