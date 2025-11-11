# MG CLEANING PRODUCT MANAGEMENT (WPF)

## Projectbeschrijving

Het **MG Cleaning Product Management** is een WPF desktop applicatie voor het beheren van schoonmaakproducten, gebouwen, personeel (arbeiders) en bestellingen. Het systeem ondersteunt rol-gebaseerde beveiliging (Admin, Manager, Schoonmaker), voorraadbeheer, automatische productcalculatie per gebouw en soft delete op alle entiteiten.

- Auteur: Yazid
- School: Erasmushogeschool Brussel
- Schooljaar: 2025
- Contact: yaya250@live.fr

## Functionaliteiten

### Gebouwenbeheer
- Gebouw CRUD (naam, oppervlakte, aantal toiletten)
- Koppeling van schoonmakers aan gebouwen (many-to-many)
- Automatische berekening benodigde producten & personeel

### Product / Voorraadbeheer
- Product CRUD (naam, verpakking, voorraad, eenheid)
- Voorraadaanpassing bij bestellingen
- Soft delete i.p.v. fysieke verwijdering

### Personeelsbeheer
- Arbeider CRUD
- Koppelen/ontkoppelen aan gebouwen via dialoog
- Overzicht per gebouw

### Bestellingen
- Bestelling CRUD per gebouw
- Selectie van producten + hoeveelheden
- Voorraadcontrole bij aanmaken / aanpassen
- Overzicht van alle bestellingen (filterbaar)

### Calculator
- Automatische berekening van benodigde producten op basis van:
  - Oppervlakte
  - Aantal toiletten
  - Vooraf ingestelde verbruiksregels
- Output direct te gebruiken voor bestelling

### Gebruikersbeheer (Identity)
- Rollen: Admin / Manager / Schoonmaker
- Wachtwoorden via User Secrets (niet in code)
- Admin kan gebruikers bewerken, blokkeren, rol wijzigen (niet eigen rol)

### Security & Data
- ASP.NET Core Identity (SQLite)
- User Secrets voor seed-wachtwoorden
- Soft delete via `IsDeleted` + globale query filters

## Gebruikersrollen

### Admin
- Volledige toegang
- Gebruikersbeheer (rollen wijzigen, blokkeren)
- Calculator toegang

### Manager
- CRUD op gebouwen, producten, personeel, bestellingen
- Arbeiders koppelen
- Calculator toegang

### Schoonmaker
- Nieuwe bestellingen plaatsen
- Eigen profiel bewerken
- Bestellingen inzien (read-only)

## Techniek
- .NET 9, C# 13, WPF (XAML)
- Entity Framework Core 9 (SQLite provider)
- ASP.NET Core Identity integratie
- MVVM architectuurprincipes
- Dependency Injection voor services
- Soft delete pattern

## Projectstructuur (vereenvoudigd)
```
MG-cleaning/
├─ MGCleaning.Desktop/                # WPF UI + Startup project
│  ├─ App.xaml(.cs)                    # Applicatie resources
│  ├─ MainWindow.xaml(.cs)             # Hoofdvenster (navigatie)
│  ├─ Views/                           # Vensters & Dialogen
│  │  ├─ LoginWindow.xaml(.cs)
│  │  ├─ AccountWindow.xaml(.cs)
│  │  ├─ GebouwenWindow.xaml(.cs)
│  │  ├─ ProductenWindow.xaml(.cs)
│  │  ├─ PersoneelWindow.xaml(.cs)
│  │  ├─ BestellingenWindow.xaml(.cs)
│  │  ├─ CalculatorWindow.xaml(.cs)
│  │  ├─ Dialogs (Edit/Koppel/Selecteer)
│  │  │  ├─ GebouwEditDialog.xaml(.cs)
│  │  │  ├─ ProductEditDialog.xaml(.cs)
│  │  │  ├─ ArbeiderEditDialog.xaml(.cs)
│  │  │  ├─ BestellingEditDialog.xaml(.cs)
│  │  │  ├─ UserEditDialog.xaml(.cs)
│  │  │  ├─ KoppelGebouwDialog.xaml(.cs)
│  │  │  └─ SelecteerGebouwDialog.xaml(.cs)
│  ├─ Services/
│  │  └─ AuthService.cs                # Authenticatie / autorisatie helpers
│  └─ appsettings.* (User Secrets voor gevoelige data)
│
├─ MGCleaning.Models/                  # Domein + EF Core
│  ├─ ApplicationDbContext.cs          # DbContext + filters
│  ├─ ApplicationDbContextFactory.cs   # Design-time factory (migrations)
│  ├─ DbSeeder.cs                      # Initiale seed (rollen & users)
│  ├─ ApplicationUser.cs               # Identity user uitbreiding
│  ├─ Gebouw.cs                        # Gebouwen
│  ├─ Product.cs                       # Producten
│  ├─ Arbeider.cs                      # Personeel
│  ├─ GebouwArbeider.cs                # Junction entity (many-to-many)
│  ├─ Bestelling.cs                    # Bestellingen
│  └─ (Migrations/)                    # EF Core migraties
│
└─ README.md
```

## Installatie & Setup
1. Vereisten: Windows 10/11, .NET 9 SDK, Visual Studio 2022+
2. Repository clonen
3. `dotnet restore`
4. User Secrets initialiseren:
   ```bash
   cd MGCleaning.Desktop
   dotnet user-secrets init
   dotnet user-secrets set "SeedPasswords:Admin" "admin123"
   dotnet user-secrets set "SeedPasswords:Manager" "manager123"
   dotnet user-secrets set "SeedPasswords:Schoonmaker" "schoonmaker123"
   ```
5. Database migraties toepassen:
   ```bash
   cd MGCleaning.Models
   dotnet ef database update --startup-project ../MGCleaning.Desktop
   ```
6. Start applicatie:
   ```bash
   dotnet run --project MGCleaning.Desktop
   ```

## Testaccounts
| Gebruikersnaam | Rol        | Wachtwoord  |
|----------------|------------|-------------|
| admin          | Admin      | admin123    |
| manager        | Manager    | manager123  |
| schoonmaker    | Schoonmaker| schoonmaker123 |

(Wachtwoorden via User Secrets, niet committen.)

## NuGet Packages
| Package | Versie | Licentie | Gebruik |
|---------|--------|----------|---------|
| Microsoft.EntityFrameworkCore.Sqlite | 9.0.0 | MIT | SQLite provider ORM |
| Microsoft.EntityFrameworkCore.Design | 9.0.10 | MIT | Migrations tooling |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.0 | Apache 2.0 | Identity store |
| Microsoft.Extensions.DependencyInjection | 9.0.10 | MIT | DI container |
| Microsoft.Extensions.Configuration.UserSecrets | 9.0.0 | MIT | Secrets beheer |
| Microsoft.AspNetCore.Identity | 2.2.0 | Apache 2.0 | Identity basis API |

**Database**: SQLite (bestand: `MGCleaning.Desktop/mgcleaning.db`)

## AI-gegenereerde Code
Dit project gebruikt **GitHub Copilot** voor:
- Code suggesties (LINQ, async/await, patterns)
- XAML binding & layout hints
- Refactoring voorstellen
- README generatie

## Troubleshooting
| Probleem | Oplossing |
|----------|-----------|
| User Secrets niet geladen | `dotnet user-secrets list` uitvoeren in Desktop project |
| Database corrupt | Verwijder `mgcleaning.db` en run opnieuw `ef database update` |
| Migrations problemen | Controleer `ApplicationDbContextFactory` en startproject vlag |

## Licentie & Copyright
- Alle eigen code: Educatief gebruik (schoolopdracht EHB)
- Externe libraries: Respecteren hun permissive licenties (MIT / Apache 2.0)
- Geen hardcoded wachtwoorden of gevoelige gegevens in repository

©2025 MG Cleaning. Alle rechten voorbehouden.
