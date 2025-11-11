using System.Windows;
using MGCleaning.Desktop.Services;
using MGCleaning.Models;

namespace MGCleaning.Desktop.Views;

public partial class ArbeiderEditDialog : Window
{
    private readonly ArbeiderService _arbeiderService;
    private readonly Arbeider? _arbeider;

    public ArbeiderEditDialog(ArbeiderService arbeiderService, Arbeider? arbeider = null)
    {
        InitializeComponent();
        _arbeiderService = arbeiderService;
        _arbeider = arbeider;

        if (_arbeider != null)
        {
            NaamTextBox.Text = _arbeider.Naam;
            TelefoonnummerTextBox.Text = _arbeider.Telefoonnummer;
            AdresTextBox.Text = _arbeider.Adres;
            IsVerantwoordelijkeCheckBox.IsChecked = _arbeider.IsVerantwoordelijke;
            Title = "Arbeider Wijzigen";
        }
        else
        {
            Title = "Nieuwe Arbeider";
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

            if (string.IsNullOrWhiteSpace(TelefoonnummerTextBox.Text))
            {
                MessageBox.Show("Telefoonnummer is verplicht.", "Validatie", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_arbeider != null)
            {
                // Wijzigen
                _arbeider.Naam = NaamTextBox.Text;
                _arbeider.Telefoonnummer = TelefoonnummerTextBox.Text;
                _arbeider.Adres = AdresTextBox.Text;
                _arbeider.IsVerantwoordelijke = IsVerantwoordelijkeCheckBox.IsChecked ?? false;
                await _arbeiderService.BijwerkenAsync(_arbeider);
            }
            else
            {
                // Toevoegen
                var nieuweArbeider = new Arbeider
                {
                    Naam = NaamTextBox.Text,
                    Telefoonnummer = TelefoonnummerTextBox.Text,
                    Adres = AdresTextBox.Text,
                    IsVerantwoordelijke = IsVerantwoordelijkeCheckBox.IsChecked ?? false
                };
                await _arbeiderService.ToevoegenAsync(nieuweArbeider);
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
