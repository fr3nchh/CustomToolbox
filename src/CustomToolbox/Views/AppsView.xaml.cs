using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CustomToolbox.Models;
using CustomToolbox.ViewModels;

namespace CustomToolbox.Views;

public partial class AppsView : UserControl
{
    private AppsViewModel? _viewModel;
    private AppInfo? _currentDetailApp;

    public AppsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _viewModel = DataContext as AppsViewModel;
    }

    private void OnCategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox lb && lb.SelectedItem is ListBoxItem item && item.Tag is string catName && _viewModel != null)
        {
            _viewModel.FilterByCategory(catName);
            AppScrollViewer.ScrollToTop();
        }
    }

    private void OnCardClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is AppInfo app)
        {
            _currentDetailApp = app;
            ShowAppDetail(app);
        }
    }

    private void ShowAppDetail(AppInfo app)
    {
        DetailName.Text = app.Name;
        DetailCategory.Text = app.Category;
        DetailPackageId.Text = app.PackageId;
        DetailDescription.Text = app.Description;
        DetailStatus.Visibility = Visibility.Collapsed;

        try
        {
            if (!string.IsNullOrEmpty(app.IconUrl))
            {
                var converter = new Converters.AppIconConverter();
                var image = converter.Convert(app.IconUrl, typeof(BitmapImage), null!, CultureInfo.InvariantCulture) as BitmapImage;
                if (image != null)
                {
                    DetailIcon.Source = image;
                    DetailIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    DetailIcon.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                DetailIcon.Visibility = Visibility.Collapsed;
            }
        }
        catch
        {
            DetailIcon.Visibility = Visibility.Collapsed;
        }

        AppDetailOverlay.Visibility = Visibility.Visible;
    }

    private void OnCloseOverlay(object sender, MouseButtonEventArgs e)
    {
        AppDetailOverlay.Visibility = Visibility.Collapsed;
        _currentDetailApp = null;
    }

    private void OnCloseOverlayBg(object sender, MouseButtonEventArgs e)
    {
        AppDetailOverlay.Visibility = Visibility.Collapsed;
        _currentDetailApp = null;
    }

    private void OnDetailPopupClick(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
    }

    private void OnDetailInstallClick(object sender, MouseButtonEventArgs e)
    {
        if (_currentDetailApp == null) return;

        var pkg = _currentDetailApp.PackageId;
        DetailStatus.Text = $"Ouverture du terminal pour installer {_currentDetailApp.Name}...";
        DetailStatus.Visibility = Visibility.Visible;

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoExit -Command \"Start-Process powershell -ArgumentList '-NoProfile -Command winget install --id {pkg} --silent --accept-package-agreements --accept-source-agreements' -Verb RunAs\"",
                UseShellExecute = false
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            DetailStatus.Text = $"Erreur: {ex.Message}";
        }
    }

    private void OnDetailLearnMoreClick(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;

        if (_currentDetailApp == null) return;

        var url = _currentDetailApp.DocumentationUrl;
        if (string.IsNullOrEmpty(url))
            url = _currentDetailApp.Website;

        if (string.IsNullOrEmpty(url)) return;

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch { }
    }

    private void OnPackageIdClick(object sender, MouseButtonEventArgs e)
    {
        if (_currentDetailApp == null) return;

        var url = _currentDetailApp.Website;
        if (string.IsNullOrEmpty(url))
            url = $"https://winget.run/pkg/{_currentDetailApp.PackageId.Replace('.', '/')}";

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch { }
    }

    private void OnInstallClick(object sender, RoutedEventArgs e)
    {
    }

    private void OnIconImageFailed(object sender, ExceptionRoutedEventArgs e)
    {
        if (sender is Image img)
        {
            img.Visibility = Visibility.Collapsed;
        }
    }

    private void OnIconImageOpened(object sender, RoutedEventArgs e)
    {
    }

    private void OnAllCategories_Click(object sender, RoutedEventArgs e)
    {
        CategoryList.SelectedItem = null;
        _viewModel?.ClearCategory();
        AppScrollViewer?.ScrollToTop();
    }

    private void OnClearFilter_Click(object sender, RoutedEventArgs e)
    {
        CategoryList.SelectedItem = null;
        _viewModel?.ClearCategory();
        AppScrollViewer?.ScrollToTop();
    }
}
