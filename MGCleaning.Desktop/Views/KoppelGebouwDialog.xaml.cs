using System.Windows;
using MGCleaning.Desktop.Services;
using MGCleaning.Models;

namespace MGCleaning.Desktop.Views;

public partial class KoppelGebouwDialog : Window
{
    private readonly ArbeiderService _arbeiderService;
    private readonly Arbeider _arbeider;

    public KoppelGebouwDialog(ArbeiderService arbeiderService, Arbeider arbeider)
    {
        InitializeComponent();
        _arbeiderService = arbeiderService;
        _arbeider = arbeider;

        ArbeiderInfoTextBlock.Text = $"Arbeider: {_arbeider.Naam}";
        Loaded += KoppelGebouwDialog_Loaded;
    }

    private async void KoppelGebouwDialog_Loaded(object sender, RoutedEventArgs e)
    {
        await LaadGekoppeldeGebouwenAsync();
    }

    private async Task LaadGekoppeldeGebouwenAsync()
    {
        try
        {
            var arbeider = await _arbeiderService.GetArbeiderByIdAsync(_arbeider.Id);
            if (arbeider != null)
            {
                GebouwenListBox.ItemsSource = arbeider.GebouwArbeiders;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fout bij laden gebouwen: {ex.Message}", "Fout", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void KoppelButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SelecteerGebouwDialog(_arbeiderService, _arbeider);
        if (dialog.ShowDialog() == true)
        {
            _ = LaadGekoppeldeGebouwenAsync();
        }
    }

    private async void OntkoppelButton_Click(object sender, RoutedEventArgs e)
    {
        if (GebouwenListBox.SelectedItem is GebouwArbeider geselecteerd)
        {
            var result = MessageBox.Show($"Weet u zeker dat u de koppeling met '{geselecteerd.Gebouw.Naam}' wilt verwijderen?", 
                "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _arbeiderService.OntkoppelVanGebouwAsync(_arbeider.Id, geselecteerd.GebouwId);
                    await LaadGekoppeldeGebouwenAsync();
                    MessageBox.Show("Koppeling succesvol verwijderd.", "Succes", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fout bij ontkoppelen: {ex.Message}", "Fout", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Selecteer eerst een gebouw.", "Waarschuwing", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void SluitenButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}
