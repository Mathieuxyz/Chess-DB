using System;
using System.Collections.Generic;

namespace ChessDB.Model
{
    public class Player
    {
        public Guid Id { get; set; }
        public string ShortId => Id.ToString("N")[..8];
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

    }

}
