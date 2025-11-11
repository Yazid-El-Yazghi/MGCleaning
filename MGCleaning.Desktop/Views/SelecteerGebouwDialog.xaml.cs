using System.Windows;
using MGCleaning.Desktop.Services;
using MGCleaning.Models;

namespace MGCleaning.Desktop.Views;

public partial class SelecteerGebouwDialog : Window
{
    private readonly ArbeiderService _arbeiderService;
    private readonly Arbeider _arbeider;
    private readonly ApplicationDbContext _context;

    public SelecteerGebouwDialog(ArbeiderService arbeiderService, Arbeider arbeider)
    {
        InitializeComponent();
        _arbeiderService = arbeiderService;
        _arbeider = arbeider;
        
        // Get context via reflection (simplified approach)
        var field = typeof(ArbeiderService).GetField("_context", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        _context = (ApplicationDbContext)field!.GetValue(_arbeiderService)!;
        
        Loaded += SelecteerGebouwDialog_Loaded;
    }

    private async void SelecteerGebouwDialog_Loaded(object sender, RoutedEventArgs e)
    {
        await LaadBeschikbareGebouwenAsync();
    }

    private async Task LaadBeschikbareGebouwenAsync()
    {
        try
        {
            var gebouwService = new GebouwService(_context);
            var alleGebouwen = await gebouwService.GetAlleGebouwenAsync();
            
            var arbeider = await _arbeiderService.GetArbeiderByIdAsync(_arbeider.Id);
            var gekoppeldeIds = arbeider?.GebouwArbeiders.Select(ga => ga.GebouwId).ToList() ?? new List<int>();
            
            // Query syntax voorbeeld
            var beschikbaar = from g in alleGebouwen
                              where !gekoppeldeIds.Contains(g.Id)
                              select g;
            
            GebouwenListBox.ItemsSource = beschikbaar.ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fout bij laden gebouwen: {ex.Message}", "Fout", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void KoppelenButton_Click(object sender, RoutedEventArgs e)
    {
        if (GebouwenListBox.SelectedItem is Gebouw geselecteerd)
        {
            try
            {
                await _arbeiderService.KoppelAanGebouwAsync(_arbeider.Id, geselecteerd.Id);
                MessageBox.Show("Gebouw succesvol gekoppeld.", "Succes", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij koppelen: {ex.Message}", "Fout", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Selecteer eerst een gebouw.", "Waarschuwing", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void AnnulerenButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
