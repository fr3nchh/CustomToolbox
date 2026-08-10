# Downiso

Installer et personnaliser Windows en toute simplicité. Catalogue de 150+ apps open-source, optimisation performances, et personnalisation système.

## Features

- **Catalogue** — 150+ applications open-source installables en un clic via Winget
- **Optimisation** — UAC, télémétrie, performances jeux, mises à jour, notifications
- **Personnalisation** — Thème, barre des tâches, menu Démarrer, explorateur de fichiers
- **Recherche** — Filtrage instantané par catégorie et par nom

## Quick Start

### Step 1: Télécharger

Cliquez sur **Code** > **Download ZIP** puis extrayez l'archive.

Ou clonez :
```bash
git clone https://github.com/fr3nchh/CustomToolbox.git
```

### Step 2: Ouvrir le projet

Ouvrez `src/CustomToolbox/CustomToolbox.csproj` dans **Visual Studio 2022** ou :
```bash
dotnet run --project src/CustomToolbox
```

### Step 3: Utiliser

- **Catalogue** — Parcourez les apps, cliquez sur une app puis "Installer via Winget"
- **Optimisation** — Activez/désactivez les paramètres Windows
- **Personnalisation** — Changez le thème, la barre des tâches, etc.

## Prérequis

- [Windows 10/11](https://www.microsoft.com/) (x64)
- [.NET 8 SDK](https://dotnet.microsoft.com/) (pour compiler)
- [Winget](https://github.com/microsoft/winget-cli) (installé automatiquement)

## Build

```bash
# Release
dotnet publish src/CustomToolbox -c Release -r win-x64 --self-contained true

# Le .exe sera dans src/CustomToolbox/bin/Release/net8.0-windows/win-x64/publish/
```

## Tech stack

- **UI** : WPF (.NET 8), XAML, Dark Theme custom
- **Architecture** : MVVM (Models, ViewModels, Views, Services)
- **Package Manager** : Winget CLI
- **Langage** : C#

## Structure

```
src/CustomToolbox/
├── Models/          # AppInfo, Category, ToggleSetting...
├── ViewModels/      # AppsViewModel, OptimizeViewModel, CustomizeViewModel
├── Views/           # AppsView, OptimizeView, CustomizeView, SettingsView
├── Services/        # WingetService, RegistryService, PowerShellService, IconService
├── Converters/      # AppIconConverter, CategoryToIconConverter...
├── Commands/        # RelayCommand
└── Resources/       # apps.json (catalogue), icons, styles, thème
```

## Author

Made by **fr4ncais**

GitHub: [https://github.com/fr4ncais](https://github.com/fr4ncais)

## License

Free project for personal use.
