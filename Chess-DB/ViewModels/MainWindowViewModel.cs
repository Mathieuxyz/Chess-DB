using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ChessDB.Model;
using System;
using System.Linq;

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
    private bool isGameDetailPage;

    [ObservableProperty]
    private Game? selectedGame;

    [ObservableProperty]
    private Player? selectedWhitePlayer;

    [ObservableProperty]
    private Player? selectedBlackPlayer;

    // Selected player for the detail view.
    [ObservableProperty]
    private Player? selectedPlayer;

    // Form fields for adding a player.
    [ObservableProperty] private string newFirstName = string.Empty;
    [ObservableProperty] private string newLastName = string.Empty;
    [ObservableProperty] private string newEmail = string.Empty;
    [ObservableProperty] private string newPhoneNumber = string.Empty;
    [ObservableProperty] private double newElo = 1500;

    // Temporary in-memory list to visualize the layout; real data wiring comes later.
    public ObservableCollection<Player> Players { get; } = new()
    {
        new Player
        {
            FirstName = "Alice", LastName = "Dubois", Email = "alice@example.com", PhoneNumber = "0600000001", CurrentElo = 1850,
            EloRatings = new()
            {
                new EloRating { Date = DateTime.Today.AddMonths(-3), Rating = 1780 },
                new EloRating { Date = DateTime.Today.AddMonths(-2), Rating = 1820 },
                new EloRating { Date = DateTime.Today.AddMonths(-1), Rating = 1845 },
                new EloRating { Date = DateTime.Today, Rating = 1850 },
            }
        },
        new Player
        {
            FirstName = "Bob", LastName = "Martin", Email = "bob@example.com", PhoneNumber = "0600000002", CurrentElo = 1720,
            EloRatings = new()
            {
                new EloRating { Date = DateTime.Today.AddMonths(-3), Rating = 1650 },
                new EloRating { Date = DateTime.Today.AddMonths(-2), Rating = 1680 },
                new EloRating { Date = DateTime.Today.AddMonths(-1), Rating = 1705 },
                new EloRating { Date = DateTime.Today, Rating = 1720 },
            }
        },
        new Player
        {
            FirstName = "Chloé", LastName = "Durand", Email = "chloe@example.com", PhoneNumber = "0600000003", CurrentElo = 1995,
            EloRatings = new()
            {
                new EloRating { Date = DateTime.Today.AddMonths(-3), Rating = 1900 },
                new EloRating { Date = DateTime.Today.AddMonths(-2), Rating = 1930 },
                new EloRating { Date = DateTime.Today.AddMonths(-1), Rating = 1970 },
                new EloRating { Date = DateTime.Today, Rating = 1995 },
            }
        },
        new Player
        {
            FirstName = "David", LastName = "Leroy", Email = "david@example.com", PhoneNumber = "0600000004", CurrentElo = 1620,
            EloRatings = new()
            {
                new EloRating { Date = DateTime.Today.AddMonths(-3), Rating = 1500 },
                new EloRating { Date = DateTime.Today.AddMonths(-2), Rating = 1540 },
                new EloRating { Date = DateTime.Today.AddMonths(-1), Rating = 1580 },
                new EloRating { Date = DateTime.Today, Rating = 1620 },
            }
        },
    };

    public ObservableCollection<Competition> Competitions { get; } = new()
    {
        new Competition
        {
            Name = "Championnat bel",
            StartDate = DateTime.Today.AddDays(7),
            EndDate = DateTime.Today.AddDays(9),
            Games =
            {
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.WhiteWin, PlayedOn = DateTime.Today.AddDays(-1) },
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.Draw, PlayedOn = DateTime.Today.AddDays(-2) },
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.Draw, PlayedOn = DateTime.Today.AddDays(-2) },
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.Draw, PlayedOn = DateTime.Today.AddDays(-2) },
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.Draw, PlayedOn = DateTime.Today.AddDays(-2) },
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.Draw, PlayedOn = DateTime.Today.AddDays(-2) },
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.Draw, PlayedOn = DateTime.Today.AddDays(-2) },
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.Draw, PlayedOn = DateTime.Today.AddDays(-2) },
            }
        },
        new Competition
        {
            Name = "Tournoi Blitz",
            StartDate = DateTime.Today.AddDays(14),
            EndDate = DateTime.Today.AddDays(14),
            Games =
            {
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.NotPlayedYet, PlayedOn = DateTime.Today.AddDays(1) },
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.BlackWin, PlayedOn = DateTime.Today },
            }
        },
        new Competition
        {
            Name = "Championnat Régional",
            StartDate = DateTime.Today.AddDays(30),
            EndDate = DateTime.Today.AddDays(33),
            Games =
            {
                new Game { WhitePlayerId = Guid.NewGuid(), BlackPlayerId = Guid.NewGuid(), Result = GameResult.WhiteWin, PlayedOn = DateTime.Today.AddDays(-5) },
            }
        },
    };

    public ObservableCollection<Registration> Registrations { get; } = new()
    {

    };

    private void SetPage(
        string text,
        bool players = false,
        bool competitions = false,
        bool registrations = false,
        bool gameDetail = false)
    {
        ContentText = text;
        IsPlayersPage = players;
        IsCompetitionsPage = competitions;
        IsRegistrationsPage = registrations;
        IsGameDetailPage = gameDetail;
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
        SelectedPlayer ??= Players.FirstOrDefault();
    }

    [RelayCommand]
    private void ShowRegistrations()
    {
        SetPage("Registrations page", registrations: true);
    }

    [RelayCommand]
    private void ShowGameDetail(Game game)
    {
        SelectedGame = game;
        SelectedWhitePlayer = Players.FirstOrDefault();
        SelectedBlackPlayer = Players.Skip(1).FirstOrDefault() ?? Players.FirstOrDefault();
        SetPage(string.Empty, gameDetail: true);
    }

    [RelayCommand]
    private void BackToCompetitions()
    {
        SelectedGame = null;
        SetPage("Competitions page", competitions: true);
    }

    [RelayCommand]
    private void ConfirmGamePlayers()
    {
        // Placeholder: hook up persistence later.
    }

    [RelayCommand]
    private void AddPlayer()
    {
        var player = new Player
        {
            Id = Guid.NewGuid(),
            FirstName = NewFirstName,
            LastName = NewLastName,
            Email = NewEmail,
            PhoneNumber = NewPhoneNumber,
            CurrentElo = NewElo,
            EloRatings = new()
        };

        Players.Add(player);
        SelectedPlayer = player;

        // Reset form
        NewFirstName = string.Empty;
        NewLastName = string.Empty;
        NewEmail = string.Empty;
        NewPhoneNumber = string.Empty;
        NewElo = 1500;
    }

    // For now, saving just reuses the add logic; replace with persistence later.
    [RelayCommand]
    private void SavePlayerForm() => AddPlayer();
}
