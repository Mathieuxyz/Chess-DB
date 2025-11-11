using System;
using System.Collections.Generic;

namespace ChessDB.Model
{
    public class Competition
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Inscriptions et parties
        public List<Registration> Registrations { get; set; } = new();
        public List<Game> Games { get; set; } = new();
    }
}
