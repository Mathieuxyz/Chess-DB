using System;

namespace ChessDB.Model
{
    public class Move
    {
        public int MoveNumber { get; set; }
        public string Notation { get; set; } // Ex: "e4", "Nf3"
    }
}
