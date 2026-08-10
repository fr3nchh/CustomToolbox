# Downiso

Installer et personnaliser Windows en toute simplicité. Catalogue de 150+ apps open-source, optimisation performances, et personnalisation système — tout-en-un.

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

### Step 2: Lancer

Double-cliquez sur **launch.bat** pour builder et lancer l'app.

Ou manuellement :
```bash
dotnet run --project src\CustomToolbox
```

## Prérequis

- [Windows 10/11](https://www.microsoft.com/) (x64)
- [.NET 8 SDK](https://dotnet.microsoft.com/) (pour compiler)
- [Winget](https://github.com/microsoft/winget-cli) (installé automatiquement)

## Build

```bash
dotnet publish src\CustomToolbox -c Release -r win-x64 --self-contained true
```

## Tech Stack

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

## License

Free project for personal use.
