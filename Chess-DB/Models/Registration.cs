using System;

namespace ChessDB.Model
{
    public class Registration
    {
        public Guid PlayerId { get; set; }
        public Guid CompetitionId { get; set; }
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Active;
    }

    public enum RegistrationStatus
    {
        Active,
        Withdrawn,
        Suspended,
        Pending,
        Completed
    }
}
