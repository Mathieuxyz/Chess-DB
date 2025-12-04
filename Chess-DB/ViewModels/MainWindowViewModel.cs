using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ChessDB.Model;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Chess_DB.ViewModels;

public partial class SelectablePlayer : ObservableObject
{
    public SelectablePlayer(Player player) => Player = player;
    public Player Player { get; }
    public Guid Id => Player.Id;
    public string Display => $"{Player.LastName}, {Player.FirstName}, {Player.ShortId}";

    [ObservableProperty]
    private bool isSelected;
}

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly DataManager _data;

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

    [ObservableProperty]
    private Competition? selectedCompetitionForRegistration;

    // Selected player for the detail view.
    [ObservableProperty]
    private Player? selectedPlayer;

    // Form fields for adding a player.
    [ObservableProperty] private string newFirstName = string.Empty;
    [ObservableProperty] private string newLastName = string.Empty;
    [ObservableProperty] private string newEmail = string.Empty;
    [ObservableProperty] private string newPhoneNumber = string.Empty;
    [ObservableProperty] private double newElo = 1500;

    // Form fields for creating a competition.
    [ObservableProperty] private string newCompetitionName = string.Empty;
    [ObservableProperty] private DateTime newCompetitionStartDate = DateTime.Today;
    [ObservableProperty] private DateTime newCompetitionEndDate = DateTime.Today.AddDays(1);

    public ObservableCollection<Player> Players { get; }

    public ObservableCollection<Competition> Competitions { get; }

    public ObservableCollection<Registration> Registrations { get; }

    public ObservableCollection<SelectablePlayer> RegistrablePlayers { get; }

    public ObservableCollection<string> MovePieces { get; } = new()
    {
        "King",
        "Queen",
        "Rook 1", "Rook 2",
        "Bishop 1", "Bishop 2",
        "Knight 1", "Knight 2",
        "Pawn 1", "Pawn 2", "Pawn 3", "Pawn 4", "Pawn 5", "Pawn 6", "Pawn 7", "Pawn 8"
    };

    public ObservableCollection<string> MoveSquares { get; } = new()
    {
        "A1","A2","A3","A4","A5","A6","A7","A8",
        "B1","B2","B3","B4","B5","B6","B7","B8",
        "C1","C2","C3","C4","C5","C6","C7","C8",
        "D1","D2","D3","D4","D5","D6","D7","D8",
        "E1","E2","E3","E4","E5","E6","E7","E8",
        "F1","F2","F3","F4","F5","F6","F7","F8",
        "G1","G2","G3","G4","G5","G6","G7","G8",
        "H1","H2","H3","H4","H5","H6","H7","H8"
    };

    [ObservableProperty]
    private string? selectedMovePiece;

    [ObservableProperty]
    private string? selectedMoveSquare;

    public MainWindowViewModel() : this(new DataManager())
    {
    }

    public MainWindowViewModel(DataManager data)
    {
        _data = data;

        Players = new ObservableCollection<Player>(_data.Players ?? new List<Player>());
        Competitions = new ObservableCollection<Competition>(_data.Competitions ?? new List<Competition>());
        Registrations = new ObservableCollection<Registration>(
            Competitions.SelectMany(c => c.Registrations));
        RegistrablePlayers = new ObservableCollection<SelectablePlayer>(
            Players.Select(p => new SelectablePlayer(p)));

        // Ensure games carry their parent competition id after loading.
        foreach (var competition in Competitions)
        {
            foreach (var game in competition.Games)
            {
                if (game.CompetitionId == Guid.Empty)
                {
                    game.CompetitionId = competition.Id;
                }
            }
        }
    }

    public static string FormatPlayerDisplay(Player p) => $"{p.LastName}, {p.FirstName}, {p.ShortId}";

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
        SelectedPlayer = null;
    }

    [RelayCommand]
    private void ShowRegistrations()
    {
        SetPage("Registrations page", registrations: true);
        SelectedCompetitionForRegistration ??= Competitions.FirstOrDefault();
        foreach (var sp in RegistrablePlayers)
        {
            sp.IsSelected = false;
        }
    }

    [RelayCommand]
    private void ShowGameDetail(Game game)
    {
        SelectedGame = game;
        SelectedWhitePlayer = GetRegisteredPlayersForCompetition(game.CompetitionId).FirstOrDefault();
        SelectedBlackPlayer = GetRegisteredPlayersForCompetition(game.CompetitionId).Skip(1).FirstOrDefault()
                               ?? SelectedWhitePlayer;
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
    private void AddRegistration()
    {
        if (SelectedCompetitionForRegistration is null)
        {
            return;
        }

        var selectedPlayers = RegistrablePlayers.Where(sp => sp.IsSelected).Select(sp => sp.Player).ToList();
        foreach (var player in selectedPlayers)
        {
            if (SelectedCompetitionForRegistration.Registrations.Any(r => r.PlayerId == player.Id))
                continue;

            var reg = new Registration
            {
                PlayerId = player.Id,
                CompetitionId = SelectedCompetitionForRegistration.Id,
                Status = RegistrationStatus.Active
            };

            SelectedCompetitionForRegistration.Registrations.Add(reg);
            Registrations.Add(reg);
        }

        // Clear selections after submit
        foreach (var sp in RegistrablePlayers)
        {
            sp.IsSelected = false;
        }
    }

    private IEnumerable<Player> GetRegisteredPlayersForCompetition(Guid competitionId)
    {
        var regIds = Competitions.FirstOrDefault(c => c.Id == competitionId)?
            .Registrations.Select(r => r.PlayerId).ToHashSet() ?? new HashSet<Guid>();

        return Players.Where(p => regIds.Contains(p.Id));
    }

    [RelayCommand(CanExecute = nameof(CanAddPlayer))]
    private void AddPlayer()
    {
        var player = new Player
        {
            Id = GenerateUniquePlayerId(),
            FirstName = NewFirstName,
            LastName = NewLastName,
            Email = NewEmail,
            PhoneNumber = NewPhoneNumber,
            CurrentElo = NewElo,
            EloRatings = new()
        };

        Players.Add(player);
        RegistrablePlayers.Add(new SelectablePlayer(player));
        // Do not auto-select; let user pick from the list.

        // Reset form
        NewFirstName = string.Empty;
        NewLastName = string.Empty;
        NewEmail = string.Empty;
        NewPhoneNumber = string.Empty;
        NewElo = 1500;
    }

    // For now, saving just reuses the add logic; replace with persistence later.
    [RelayCommand(CanExecute = nameof(CanAddPlayer))]
    private void SavePlayerForm() => AddPlayer();

    private bool CanAddPlayer()
    {
        return !string.IsNullOrWhiteSpace(NewFirstName)
               && !string.IsNullOrWhiteSpace(NewLastName)
               && !string.IsNullOrWhiteSpace(NewEmail)
               && !string.IsNullOrWhiteSpace(NewPhoneNumber);
    }

    partial void OnNewFirstNameChanged(string value) => AddPlayerCommand.NotifyCanExecuteChanged();
    partial void OnNewLastNameChanged(string value) => AddPlayerCommand.NotifyCanExecuteChanged();
    partial void OnNewEmailChanged(string value) => AddPlayerCommand.NotifyCanExecuteChanged();
    partial void OnNewPhoneNumberChanged(string value) => AddPlayerCommand.NotifyCanExecuteChanged();

    private Guid GenerateUniquePlayerId()
    {
        var existingShortIds = Players.Select(p => p.ShortId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        Guid candidate;
        string shortId;
        do
        {
            candidate = Guid.NewGuid();
            shortId = candidate.ToString("N")[..8];
        } while (existingShortIds.Contains(shortId));

        return candidate;
    }

    [RelayCommand]
    private void AddCompetition()
    {
        if (string.IsNullOrWhiteSpace(NewCompetitionName))
        {
            return;
        }

        var competition = new Competition
        {
            Name = NewCompetitionName.Trim(),
            StartDate = NewCompetitionStartDate,
            EndDate = NewCompetitionEndDate
        };

        Competitions.Add(competition);
        SelectedCompetitionForRegistration = competition;

        // Reset form
        NewCompetitionName = string.Empty;
        NewCompetitionStartDate = DateTime.Today;
        NewCompetitionEndDate = DateTime.Today.AddDays(1);
    }
}
