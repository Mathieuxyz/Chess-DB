using System;
using System.Collections.ObjectModel;

namespace ChessDB.Model
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CompetitionId { get; set; }
        public Guid WhitePlayerId { get; set; }
        public Guid BlackPlayerId { get; set; }
        public int MatchNumber { get; set; }
        public GameResult Result { get; set; } = GameResult.NotPlayedYet;
        public DateTime PlayedOn { get; set; } = DateTime.Now;

        // List of moves played
        public ObservableCollection<Move> Moves { get; set; } = new();
    }

    public enum GameResult
    {
        WhiteWin,
        BlackWin,
        Draw,
        NotPlayedYet
    }
}
