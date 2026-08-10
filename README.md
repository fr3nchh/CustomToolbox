# Downiso

Install and customize Windows with ease. A catalog of 150+ open-source apps, performance optimization, and system customization — all in one tool.

## Features

- **Catalog** — 150+ open-source apps installable in one click via Winget
- **Optimization** — UAC, telemetry, gaming performance, updates, notifications
- **Customization** — Theme, taskbar, Start Menu, File Explorer
- **Search** — Instant filtering by category and name

## Quick Start

### Option 1: Download the .exe

Download **Downiso.exe** from the repo root and run it. No build needed.

### Option 2: Build from source

Clone and build:
```bash
git clone https://github.com/fr3nchh/CustomToolbox.git
cd CustomToolbox
dotnet run --project src\CustomToolbox
```

## Prerequisites (for building from source only)

- [Windows 10/11](https://www.microsoft.com/) (x64)
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Winget](https://github.com/microsoft/winget-cli) (installed automatically)

## Build

```bash
dotnet publish src\CustomToolbox -c Release -r win-x64 --self-contained true
```

## Tech Stack

- **UI**: WPF (.NET 8), XAML, Custom Dark Theme
- **Architecture**: MVVM (Models, ViewModels, Views, Services)
- **Package Manager**: Winget CLI
- **Language**: C#

## Structure

```
src/CustomToolbox/
├── Models/          # AppInfo, Category, ToggleSetting...
├── ViewModels/      # AppsViewModel, OptimizeViewModel, CustomizeViewModel
├── Views/           # AppsView, OptimizeView, CustomizeView, SettingsView
├── Services/        # WingetService, RegistryService, PowerShellService, IconService
├── Converters/      # AppIconConverter, CategoryToIconConverter...
├── Commands/        # RelayCommand
└── Resources/       # apps.json (catalog), icons, styles, theme
```

## License

Free project for personal use.
