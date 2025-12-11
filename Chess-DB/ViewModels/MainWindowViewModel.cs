using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ChessDB.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chess_DB.Services;

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

public partial class MoveForm : ObservableObject
{
    [ObservableProperty] private Player? player;
    [ObservableProperty] private string? piece;
    [ObservableProperty] private string? square;
    [ObservableProperty] private bool isSaved;
}

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly DataManager _data;

    public string ProgramName => _data.Settings.ProgramName;
    public string RankingName => _data.Settings.RankingName;
    public string TopRankingPlayersTitle => $"Top {RankingName} players";
    public string RankingHistoryTitle => $"{RankingName} history";
    public string CurrentRankingWatermark => $"Current {RankingName}";

    // Text shown in the main content area.
    [ObservableProperty]
    private string contentText = "Select an action from the menu.";

    [ObservableProperty]
    private bool isHomePage = true;

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
    private bool isResetPage;

    [ObservableProperty]
    private bool showResetConfirmation;

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
    [ObservableProperty] private DateTimeOffset? newCompetitionStartDate = DateTimeOffset.Now.Date;
    [ObservableProperty] private DateTimeOffset? newCompetitionEndDate = DateTimeOffset.Now.Date.AddDays(1);
    [ObservableProperty] private int newCompetitionMatchCount = 1;

    public ObservableCollection<Player> Players { get; }

    public ObservableCollection<Competition> Competitions { get; }

    public ObservableCollection<Registration> Registrations { get; }

    public ObservableCollection<SelectablePlayer> RegistrablePlayers { get; }
    public ObservableCollection<Player> MovePlayers { get; } = new();
    public ObservableCollection<MoveForm> MoveForms { get; } = new();
    public ObservableCollection<string> MovePieces { get; }
    public ObservableCollection<string> MoveSquares { get; }

    public IEnumerable<Competition> UpcomingCompetitions =>
        Competitions
            .Where(c => c.StartDate >= DateTime.Today)
            .OrderBy(c => c.StartDate)
            .Take(5);

    public IEnumerable<Player> TopPlayers =>
        Players
            .OrderByDescending(p => p.CurrentElo)
            .Take(5);

    [ObservableProperty]
    private string? selectedMovePiece;

    [ObservableProperty]
    private string? selectedMoveSquare;

    [ObservableProperty]
    private Player? selectedMovePlayer;

    public MainWindowViewModel() : this(new DataManager())
    {
    }

    public MainWindowViewModel(DataManager data)
    {
        _data = data;
        _data.Settings.EnsureDefaults();

        Players = new ObservableCollection<Player>(_data.Players ?? new List<Player>());
        Competitions = new ObservableCollection<Competition>(_data.Competitions ?? new List<Competition>());
        Registrations = new ObservableCollection<Registration>(
            Competitions.SelectMany(c => c.Registrations));
        RegistrablePlayers = new ObservableCollection<SelectablePlayer>(
            Players.Select(p => new SelectablePlayer(p)));
        MovePieces = new ObservableCollection<string>(_data.Settings.MovePieces);
        MoveSquares = new ObservableCollection<string>(_data.Settings.MoveSquares);

        Players.CollectionChanged += (_, _) => RefreshComputedLists();
        Competitions.CollectionChanged += (_, _) => RefreshComputedLists();

        // Ensure games carry their parent competition id after loading.
        foreach (var competition in Competitions)
        {
            var idx = 1;
            foreach (var game in competition.Games)
            {
                if (game.CompetitionId == Guid.Empty)
                {
                    game.CompetitionId = competition.Id;
                }
                if (game.Moves is null)
                {
                    game.Moves = new System.Collections.ObjectModel.ObservableCollection<Move>();
                }
                if (game.MatchNumber <= 0)
                {
                    game.MatchNumber = idx;
                }
                idx++;
            }
        }
    }

    public static string FormatPlayerDisplay(Player p) => $"{p.LastName}, {p.FirstName}, {p.ShortId}";

    private void SetPage(
        string text,
        bool home = false,
        bool players = false,
        bool competitions = false,
        bool registrations = false,
        bool gameDetail = false,
        bool reset = false)
    {
        ContentText = text;
        IsHomePage = home;
        IsPlayersPage = players;
        IsCompetitionsPage = competitions;
        IsRegistrationsPage = registrations;
        IsGameDetailPage = gameDetail;
        IsResetPage = reset;
    }

    // Each command just swaps the displayed text for now.
    [RelayCommand]
    private void ShowHome()
    {
        SetPage("Welcome", home: true);
    }
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
    private void ShowReset()
    {
        SetPage("Reset everything", reset: true);
    }

    [RelayCommand]
    private void ToggleResetConfirmation()
    {
        ShowResetConfirmation = !ShowResetConfirmation;
    }

    [RelayCommand]
    private async Task ResetEverything()
    {
        Players.Clear();
        Competitions.Clear();
        Registrations.Clear();
        RegistrablePlayers.Clear();
        MovePlayers.Clear();
        MoveForms.Clear();
        SelectedPlayer = null;
        SelectedGame = null;
        SelectedBlackPlayer = null;
        SelectedWhitePlayer = null;

        _data.Players.Clear();
        _data.Competitions.Clear();

        await DataFileService.SaveAsync(_data);

        ShowResetConfirmation = false;
        RefreshComputedLists();
    }

    [RelayCommand]
    private void ShowGameDetail(Game game)
    {
        SelectedGame = game;

        // Try to keep previously saved selections, fallback to registered players.
        SelectedWhitePlayer = Players.FirstOrDefault(p => p.Id == game.WhitePlayerId)
                              ?? GetRegisteredPlayersForCompetition(game.CompetitionId).FirstOrDefault();
        SelectedBlackPlayer = Players.FirstOrDefault(p => p.Id == game.BlackPlayerId)
                              ?? GetRegisteredPlayersForCompetition(game.CompetitionId).Skip(1).FirstOrDefault()
                              ?? SelectedWhitePlayer;

        RefreshMovePlayers();
        InitializeMoveFormsFromGame(game);
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
        if (SelectedGame is null || SelectedWhitePlayer is null || SelectedBlackPlayer is null)
        {
            return;
        }

        SelectedGame.WhitePlayerId = SelectedWhitePlayer.Id;
        SelectedGame.BlackPlayerId = SelectedBlackPlayer.Id;
        RefreshMovePlayers();
    }

    [RelayCommand]
    private void AddMoveRow()
    {
        MoveForms.Add(new MoveForm());
    }

    [RelayCommand]
    private void SaveMoves()
    {
        if (SelectedGame is null)
        {
            return;
        }

        var nextNumber = SelectedGame.Moves.Count + 1;
        foreach (var form in MoveForms)
        {
            if (form.Player is null || string.IsNullOrWhiteSpace(form.Piece) || string.IsNullOrWhiteSpace(form.Square))
            {
                continue;
            }

            var notation = $"{form.Piece} {form.Square} ({form.Player.ShortId})";
            SelectedGame.Moves.Add(new Move
            {
                MoveNumber = nextNumber++,
                Notation = notation
            });

            form.IsSaved = true;
        }

        // After saving, start fresh with a single empty row.
        MoveForms.Clear();
        AddMoveRow();
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

        RefreshComputedLists();
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

        var startDate = (NewCompetitionStartDate ?? DateTimeOffset.Now.Date).Date;
        var endDate = (NewCompetitionEndDate ?? startDate.AddDays(1)).Date;
        var matchCount = Math.Max(1, NewCompetitionMatchCount);

        var competition = new Competition
        {
            Name = NewCompetitionName.Trim(),
            StartDate = startDate,
            EndDate = endDate
        };

        for (var i = 1; i <= matchCount; i++)
        {
            competition.Games.Add(new Game
            {
                CompetitionId = competition.Id,
                MatchNumber = i,
                PlayedOn = startDate
            });
        }

        Competitions.Add(competition);
        SelectedCompetitionForRegistration = competition;

        // Reset form
        NewCompetitionName = string.Empty;
        NewCompetitionStartDate = DateTimeOffset.Now.Date;
        NewCompetitionEndDate = DateTimeOffset.Now.Date.AddDays(1);
        NewCompetitionMatchCount = 1;

        RefreshComputedLists();
    }

    private void RefreshMovePlayers()
    {
        MovePlayers.Clear();
        if (SelectedWhitePlayer is not null)
        {
            MovePlayers.Add(SelectedWhitePlayer);
        }

        if (SelectedBlackPlayer is not null && SelectedBlackPlayer != SelectedWhitePlayer)
        {
            MovePlayers.Add(SelectedBlackPlayer);
        }

        SelectedMovePlayer = MovePlayers.FirstOrDefault();
    }

    private void InitializeMoveFormsFromGame(Game game)
    {
        MoveForms.Clear();
        // Start with a single blank row for new moves.
        AddMoveRow();
    }

    private void RefreshComputedLists()
    {
        OnPropertyChanged(nameof(UpcomingCompetitions));
        OnPropertyChanged(nameof(TopPlayers));
    }
}
