namespace ChessDB.Model
{
    public class Player
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Elo rating actuel
        public double CurrentElo { get; set; }

        // Historique des ratings
        public List<EloRating> EloRatings { get; set; } = new List<EloRating>();

        // Inscriptions aux compétitions
        public List<Registration> Registrations { get; set; } = new List<Registration>();

        // Parties jouées (comme blanc ou noir)
        public List<Game> GamesAsWhite { get; set; } = new List<Game>();
        public List<Game> GamesAsBlack { get; set; } = new List<Game>();
    }

    public class Competition
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Organisateur par exemple
        public string Organizer { get; set; }

        // Participations
        public List<Registration> Registrations { get; set; } = new List<Registration>();

        // Parties jouées dans cette compétition
        public List<Game> Games { get; set; } = new List<Game>();
    }

    public class Registration
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }
        public Player Player { get; set; }

        public Guid CompetitionId { get; set; }
        public Competition Competition { get; set; }

        public DateTime RegistrationDate { get; set; }

        // Statut (inscrit, désisté, suspendu, etc)
        public RegistrationStatus Status { get; set; }
    }

    public enum RegistrationStatus
    {
        Registered,
        Withdrawn,
        Suspended,
        Pending,
        Completed
    }

    public class Game
    {
        public Guid Id { get; set; }

        public Guid CompetitionId { get; set; }
        public Competition Competition { get; set; }

        public Guid WhitePlayerId { get; set; }
        public Player WhitePlayer { get; set; }

        public Guid BlackPlayerId { get; set; }
        public Player BlackPlayer { get; set; }

        public DateTime PlayedOn { get; set; }

        // Résultat (1-0, 0-1, ½-½) → on peut utiliser un enum ou struct
        public GameResult Result { get; set; }

        // Liste de coups
        public List<Move> Moves { get; set; } = new List<Move>();
    }

    public enum GameResult
    {
        WhiteWin,
        BlackWin,
        Draw,
        NotPlayedYet
    }

    public class Move
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }
        public Game Game { get; set; }

        // Numéro du coup (1, 2, 3…)
        public int MoveNumber { get; set; }

        // Notation algébrique du coup (ex : "e4", "Nf3", etc.)
        public string Notation { get; set; }

        // Temps utilisé ou autre info si souhaité
        // public TimeSpan TimeUsed { get; set; }
    }

    public class EloRating
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }
        public Player Player { get; set; }

        public DateTime Date { get; set; }
        public double Rating { get; set; }
    }
}
