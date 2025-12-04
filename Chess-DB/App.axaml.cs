using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Chess_DB.ViewModels;
using Chess_DB.Views;
using Chess_DB.Services;
using ChessDB.Model;

namespace Chess_DB;

public partial class App : Application
{
    private readonly DataManager _dataManager = DataFileService.LoadAsync().GetAwaiter().GetResult();
    private readonly MainWindowViewModel _mainViewModel;
    private bool _canClose;

    public App()
    {
        _mainViewModel = new MainWindowViewModel(_dataManager);
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainViewModel,
            };
            desktop.ShutdownRequested += OnShutdownRequested;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private async void OnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        e.Cancel = !_canClose;

        if (_canClose)
        {
            return;
        }

        _dataManager.Players = _mainViewModel.Players.ToList();
        _dataManager.Competitions = _mainViewModel.Competitions.ToList();

        await DataFileService.SaveAsync(_dataManager);

        _canClose = true;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}
