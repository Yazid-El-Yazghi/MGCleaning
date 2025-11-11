using System.Windows;
using MGCleaning.Models;
using MGCleaning.Desktop.Services;
using MGCleaning.Desktop.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace MGCleaning.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        public IServiceProvider Services => _serviceProvider!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Configureer services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Initialiseer database
            InitializeDatabaseAsync().Wait();

            // Start met LoginWindow
            var authService = _serviceProvider.GetRequiredService<AuthService>();
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            var loginWindow = new Views.LoginWindow(authService, mainWindow);
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configuration (User Secrets)
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<App>()
                .Build();
            
            services.AddSingleton<IConfiguration>(configuration);

            // Database pad - in project root (niet in bin folder)
            var dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "mgcleaning.db");
            var fullDbPath = System.IO.Path.GetFullPath(dbPath);

            // Database Context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite($"Data Source={fullDbPath}"));

            // Identity
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // Zwakkere wachtwoord requirements
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Services
            services.AddScoped<GebouwService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ArbeiderService>();
            services.AddScoped<BestellingService>();
            services.AddScoped<AuthService>();

            // ViewModels
            services.AddTransient<MainViewModel>();

            // Windows
            services.AddTransient<MainWindow>();
        }

        private async Task InitializeDatabaseAsync()
        {
            try
            {
                using var scope = _serviceProvider!.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                // Pas migraties toe
                await context.Database.MigrateAsync();

                // Seed data
                await DbSeeder.SeedAsync(context, userManager, roleManager, configuration);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij initialiseren database: {ex.Message}", "Database Fout",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Event handler voor XAML
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}
