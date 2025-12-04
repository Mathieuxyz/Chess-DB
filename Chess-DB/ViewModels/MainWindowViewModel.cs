using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ChessDB.Model;
using System;

namespace Chess_DB.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Text shown in the main content area.
    [ObservableProperty]
    private string contentText = "Select an action from the menu.";

    // Flag to know when to show the players list instead of text.
    [ObservableProperty]
    private bool isPlayersPage;

    [ObservableProperty]
    private bool isCompetitionsPage;

    [ObservableProperty]
    private bool isRegistrationsPage;

    [ObservableProperty]
    private bool isGamesPage;

    [ObservableProperty]
    private bool isEloPage;

    // Temporary in-memory list to visualize the layout; real data wiring comes later.
    public ObservableCollection<Player> Players { get; } = new()
    {
        new Player { FirstName = "Alice", LastName = "Dubois", Email = "alice@example.com", PhoneNumber = "0600000001", CurrentElo = 1850 },
        new Player { FirstName = "Bob", LastName = "Martin", Email = "bob@example.com", PhoneNumber = "0600000002", CurrentElo = 1720 },
        new Player { FirstName = "Chloé", LastName = "Durand", Email = "chloe@example.com", PhoneNumber = "0600000003", CurrentElo = 1995 },
        new Player { FirstName = "David", LastName = "Leroy", Email = "david@example.com", PhoneNumber = "0600000004", CurrentElo = 1620 },
    };

    public ObservableCollection<Competition> Competitions { get; } = new()
    {
        new Competition { Name = "Open de Paris", StartDate = DateTime.Today.AddDays(7), EndDate = DateTime.Today.AddDays(9) },
        new Competition { Name = "Tournoi Blitz", StartDate = DateTime.Today.AddDays(14), EndDate = DateTime.Today.AddDays(14) },
        new Competition { Name = "Championnat Régional", StartDate = DateTime.Today.AddDays(30), EndDate = DateTime.Today.AddDays(33) },
    };

    public ObservableCollection<Registration> Registrations { get; } = new()
    {
        new Registration { PlayerId = Guid.NewGuid(), CompetitionId = Guid.NewGuid(), Status = RegistrationStatus.Active },
        new Registration { PlayerId = Guid.NewGuid(), CompetitionId = Guid.NewGuid(), Status = RegistrationStatus.Pending },
        new Registration { PlayerId = Guid.NewGuid(), CompetitionId = Guid.NewGuid(), Status = RegistrationStatus.Completed },
    };

    public ObservableCollection<Game> Games { get; } = new()
    {
        new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), CompetitionId = Guid.NewGuid(), Result = GameResult.WhiteWin, PlayedOn = DateTime.Today.AddDays(-3) },
        new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), CompetitionId = Guid.NewGuid(), Result = GameResult.Draw, PlayedOn = DateTime.Today.AddDays(-1) },
        new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), CompetitionId = Guid.NewGuid(), Result = GameResult.NotPlayedYet, PlayedOn = DateTime.Today.AddDays(2) },
    };

    public ObservableCollection<EloRating> EloRatings { get; } = new()
    {
        new EloRating { Date = DateTime.Today.AddMonths(-3), Rating = 1800 },
        new EloRating { Date = DateTime.Today.AddMonths(-2), Rating = 1850 },
        new EloRating { Date = DateTime.Today.AddMonths(-1), Rating = 1900 },
        new EloRating { Date = DateTime.Today, Rating = 1920 },
    };

    private void SetPage(
        string text,
        bool players = false,
        bool competitions = false,
        bool registrations = false,
        bool games = false,
        bool elo = false)
    {
        ContentText = text;
        IsPlayersPage = players;
        IsCompetitionsPage = competitions;
        IsRegistrationsPage = registrations;
        IsGamesPage = games;
        IsEloPage = elo;
    }

    // Each command just swaps the displayed text for now.
    [RelayCommand]
    private void ShowCompetitions()
    {
        SetPage("Competitions page", competitions: true);
    }

    [RelayCommand]
    private void ShowPlayers()
    {
        SetPage(string.Empty, players: true);
    }

    [RelayCommand]
    private void ShowRegistrations()
    {
        SetPage("Registrations page", registrations: true);
    }

    [RelayCommand]
    private void ShowGames()
    {
        SetPage("Games page", games: true);
    }

    [RelayCommand]
    private void ShowEloRankings()
    {
        SetPage("ELO rankings page", elo: true);
    }
}
