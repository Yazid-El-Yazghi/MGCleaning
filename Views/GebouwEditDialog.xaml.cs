using System.Windows;
using MGCleaning.Desktop.Services;
using MGCleaning.Models;

namespace MGCleaning.Desktop.Views;

public partial class GebouwEditDialog : Window
{
    private readonly GebouwService _gebouwService;
    private readonly Gebouw? _gebouw;

    public GebouwEditDialog(GebouwService gebouwService, Gebouw? gebouw = null)
    {
        InitializeComponent();
        _gebouwService = gebouwService;
        _gebouw = gebouw;

        if (_gebouw != null)
        {
            NaamTextBox.Text = _gebouw.Naam;
            AdresTextBox.Text = _gebouw.Adres;
            OppervlakteTextBox.Text = _gebouw.OppervlakteM2.ToString();
            ToilettenTextBox.Text = _gebouw.AantalToiletten.ToString();
            Title = "Gebouw Wijzigen";
        }
        else
        {
            Title = "Nieuw Gebouw";
        }
    }

    private async void OpslaanButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(NaamTextBox.Text))
            {
                MessageBox.Show("Naam is verplicht.", "Validatie", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(OppervlakteTextBox.Text, out var oppervlakte) || oppervlakte <= 0)
            {
                MessageBox.Show("Voer een geldige oppervlakte in.", "Validatie", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(ToilettenTextBox.Text, out var toiletten) || toiletten < 0)
            {
                MessageBox.Show("Voer een geldig aantal toiletten in.", "Validatie", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_gebouw != null)
            {
                // Wijzigen
                _gebouw.Naam = NaamTextBox.Text;
                _gebouw.Adres = AdresTextBox.Text;
                _gebouw.OppervlakteM2 = oppervlakte;
                _gebouw.AantalToiletten = toiletten;
                await _gebouwService.BijwerkenAsync(_gebouw);
            }
            else
            {
                // Toevoegen
                var nieuwGebouw = new Gebouw
                {
                    Naam = NaamTextBox.Text,
                    Adres = AdresTextBox.Text,
                    OppervlakteM2 = oppervlakte,
                    AantalToiletten = toiletten
                };
                await _gebouwService.ToevoegenAsync(nieuwGebouw);
            }

            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fout bij opslaan: {ex.Message}", "Fout", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void AnnulerenButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
