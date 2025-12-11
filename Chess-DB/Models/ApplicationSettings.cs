using System.Collections.Generic;

namespace ChessDB.Model
{
    public class ApplicationSettings
    {
        public string ProgramName { get; set; } = "Chess contest manager";
        public string RankingName { get; set; } = "ELO";
        public List<string> MovePieces { get; set; } = CreateDefaultPieces();
        public List<string> MoveSquares { get; set; } = CreateDefaultSquares();

        public void EnsureDefaults()
        {
            if (string.IsNullOrWhiteSpace(ProgramName))
            {
                ProgramName = "Chess contest manager";
            }

            if (string.IsNullOrWhiteSpace(RankingName))
            {
                RankingName = "ELO";
            }

            if (MovePieces is null || MovePieces.Count == 0)
            {
                MovePieces = CreateDefaultPieces();
            }

            if (MoveSquares is null || MoveSquares.Count == 0)
            {
                MoveSquares = CreateDefaultSquares();
            }
        }

        public static List<string> CreateDefaultPieces() => new()
        {
            "King",
            "Queen",
            "Rook 1", "Rook 2",
            "Bishop 1", "Bishop 2",
            "Knight 1", "Knight 2",
            "Pawn 1", "Pawn 2", "Pawn 3", "Pawn 4", "Pawn 5", "Pawn 6", "Pawn 7", "Pawn 8"
        };

        public static List<string> CreateDefaultSquares() => new()
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
    }
}
