using System.Windows;
using MGCleaning.Desktop.Services;

namespace MGCleaning.Desktop.Views;

public partial class LoginWindow : Window
{
    private readonly AuthService _authService;
    private readonly MainWindow _mainWindow;

    public LoginWindow(AuthService authService, MainWindow mainWindow)
    {
        InitializeComponent();
        _authService = authService;
        _mainWindow = mainWindow;
        
        // Focus op username textbox
        Loaded += (s, e) => UsernameTextBox.Focus();
        
        // Enter key support
        PasswordBox.KeyDown += (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                LoginButton_Click(s, e);
        };
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ErrorMessageTextBlock.Text = "";

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                ErrorMessageTextBlock.Text = "Gebruikersnaam is verplicht.";
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ErrorMessageTextBlock.Text = "Wachtwoord is verplicht.";
                return;
            }

            // Probeer in te loggen
            var success = await _authService.LoginAsync(UsernameTextBox.Text, PasswordBox.Password);

            if (success)
            {
                // Open hoofdvenster en sluit login
                _mainWindow.Show();
                await _mainWindow.InitializeAfterLoginAsync();
                Close();
            }
            else
            {
                ErrorMessageTextBlock.Text = "Ongeldige inloggegevens.";
                PasswordBox.Clear();
                PasswordBox.Focus();
            }
        }
        catch (Exception ex)
        {
            ErrorMessageTextBlock.Text = $"Fout: {ex.Message}";
        }
    }
}
