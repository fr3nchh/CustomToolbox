using System.Collections.ObjectModel;
using CustomToolbox.Models;
using CustomToolbox.Services;

namespace CustomToolbox.ViewModels;

public class AppsViewModel : BaseViewModel
{
    private readonly AppCatalogService _catalogService;
    private string _searchText = string.Empty;
    private string? _selectedCategoryName;
    private bool _isLoading;
    private string _statusMessage = string.Empty;
    private List<AppInfo> _allApps = new();
    private List<Category> _allCategories = new();

    public ObservableCollection<Category> Categories { get; } = new();
    public ObservableCollection<AppInfo> FilteredApps { get; } = new();

    public string SearchText
    {
        get => _searchText;
        set { SetProperty(ref _searchText, value); FilterApps(); }
    }

    public string? SelectedCategoryName
    {
        get => _selectedCategoryName;
        set
        {
            SetProperty(ref _selectedCategoryName, value);
            FilterApps();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public AppsViewModel()
    {
        _catalogService = new AppCatalogService();
        _ = LoadCatalog();
    }

    private async Task LoadCatalog()
    {
        IsLoading = true;
        var catalog = await _catalogService.LoadCatalog();
        _allCategories = catalog.Categories;
        _allApps = catalog.Categories.SelectMany(c => c.Apps).ToList();

        Categories.Clear();
        foreach (var cat in _allCategories)
            Categories.Add(cat);

        FilterApps();
        IsLoading = false;
    }

    public void FilterByCategory(string categoryName)
    {
        SelectedCategoryName = categoryName;
    }

    public void ClearCategory()
    {
        SelectedCategoryName = null;
    }

    private void FilterApps()
    {
        FilteredApps.Clear();

        IEnumerable<AppInfo> source;

        if (!string.IsNullOrEmpty(_selectedCategoryName))
        {
            source = _allApps.Where(a =>
                string.Equals(a.Category, _selectedCategoryName, StringComparison.OrdinalIgnoreCase));

            if (!source.Any())
            {
                var cat = _allCategories.FirstOrDefault(c =>
                    string.Equals(c.Name, _selectedCategoryName, StringComparison.OrdinalIgnoreCase));
                source = cat?.Apps ?? [];
            }
        }
        else
        {
            source = _allApps;
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            source = source.Where(a =>
                a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.PackageId.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var app in source)
            FilteredApps.Add(app);

        StatusMessage = $"{FilteredApps.Count} applications disponibles";
    }
}
