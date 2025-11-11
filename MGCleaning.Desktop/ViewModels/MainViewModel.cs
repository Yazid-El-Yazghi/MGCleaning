using MGCleaning.Models;
using MGCleaning.Desktop.Services;
using System.Collections.ObjectModel;

namespace MGCleaning.Desktop.ViewModels;

/// <summary>
/// ViewModel voor het hoofdvenster
/// </summary>
public class MainViewModel : BaseViewModel
{
    private readonly GebouwService _gebouwService;
    private readonly ProductService _productService;
    private readonly ArbeiderService _arbeiderService;
    private readonly AuthService _authService;

    private ObservableCollection<Gebouw> _gebouwen = new();
    private ObservableCollection<Product> _producten = new();
    private ObservableCollection<Arbeider> _arbeiders = new();
    private string _gebruikersNaam = "Niet ingelogd";
    private bool _isIngelogd;

    public ObservableCollection<Gebouw> Gebouwen
    {
        get => _gebouwen;
        set => SetProperty(ref _gebouwen, value);
    }

    public ObservableCollection<Product> Producten
    {
        get => _producten;
        set => SetProperty(ref _producten, value);
    }

    public ObservableCollection<Arbeider> Arbeiders
    {
        get => _arbeiders;
        set => SetProperty(ref _arbeiders, value);
    }

    public string GebruikersNaam
    {
        get => _gebruikersNaam;
        set => SetProperty(ref _gebruikersNaam, value);
    }

    public bool IsIngelogd
    {
        get => _isIngelogd;
        set => SetProperty(ref _isIngelogd, value);
    }

    public MainViewModel(GebouwService gebouwService, ProductService productService, 
        ArbeiderService arbeiderService, AuthService authService)
    {
        _gebouwService = gebouwService;
        _productService = productService;
        _arbeiderService = arbeiderService;
        _authService = authService;
    }

    public async Task LaadDataAsync()
    {
        try
        {
            var gebouwen = await _gebouwService.GetAlleGebouwenAsync();
            Gebouwen = new ObservableCollection<Gebouw>(gebouwen);

            var producten = await _productService.GetAlleProductenAsync();
            Producten = new ObservableCollection<Product>(producten);

            var arbeiders = await _arbeiderService.GetAlleArbeidersAsync();
            Arbeiders = new ObservableCollection<Arbeider>(arbeiders);

            UpdateGebruikersInfo();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fout bij laden data: {ex.Message}", ex);
        }
    }

    public void UpdateGebruikersInfo()
    {
        if (_authService.HuidigeGebruiker != null)
        {
            GebruikersNaam = _authService.HuidigeGebruiker.Email ?? "Onbekend";
            IsIngelogd = true;
        }
        else
        {
            GebruikersNaam = "Niet ingelogd";
            IsIngelogd = false;
        }
    }
}
