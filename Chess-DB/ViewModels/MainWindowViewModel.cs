using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chess_DB.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Text shown in the main content area.
    [ObservableProperty]
    private string contentText = "Select an action from the menu.";

    // Each command just swaps the displayed text for now.
    [RelayCommand]
    private void ShowCompetitions() => ContentText = "Competitions page";

    [RelayCommand]
    private void ShowPlayers() => ContentText = "Player's page";

    [RelayCommand]
    private void ShowRegistrations() => ContentText = "Registrations page";

    [RelayCommand]
    private void ShowGames() => ContentText = "Games page";

    [RelayCommand]
    private void ShowEloRankings() => ContentText = "ELO rankings page";
}
