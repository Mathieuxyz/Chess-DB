using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ChessDB.Model;

namespace Chess_DB.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Text shown in the main content area.
    [ObservableProperty]
    private string contentText = "Select an action from the menu.";

    // Flag to know when to show the players list instead of text.
    [ObservableProperty]
    private bool isPlayersPage;

    // Temporary in-memory list to visualize the layout; real data wiring comes later.
    public ObservableCollection<Player> Players { get; } = new()
    {
        new Player { FirstName = "Alice", LastName = "Dubois", Email = "alice@example.com", PhoneNumber = "0600000001", CurrentElo = 1850 },
    };

    // Each command just swaps the displayed text for now.
    [RelayCommand]
    private void ShowCompetitions()
    {
        IsPlayersPage = false;
        ContentText = "Competitions page";
    }

    [RelayCommand]
    private void ShowPlayers()
    {
        IsPlayersPage = true;
        ContentText = string.Empty;
    }

    [RelayCommand]
    private void ShowRegistrations()
    {
        IsPlayersPage = false;
        ContentText = "Registrations page";
    }

    [RelayCommand]
    private void ShowGames()
    {
        IsPlayersPage = false;
        ContentText = "Games page";
    }

    [RelayCommand]
    private void ShowEloRankings()
    {
        IsPlayersPage = false;
        ContentText = "ELO rankings page";
    }
}
