using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Misconduct/commendation Register Entries
    /// </summary>
    public class OfficialRegisterEntryModel
    {

        public int Id { get; set; }
        public string OfficialRegisterId { get; set; }
        public string PersonId { get; set; }

        public string InstitutionId { get; set; }
        public int JobTitleId { get; set; }
        public int EmploymentStatusId { get; set; }

        public int EntryYear { get; set; }
        public int EntryMonth { get; set; }
        public int EntryDay { get; set; }
        public string EntryDetailsText { get; set; }

        public string AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }

        public bool IsFlaggedForModeration { get; set; }
        public int FlaggedCount { get; set; }
        public DateTime LastFlaggedDate { get; set; }
        public DateTime ModeratorReviewDate { get; set; }
        public string ModeratorUserId { get; set; }
        public int FlagActionTakenTypeId { get; set; }
        public string ModeratorNote { get; set; }

        public bool NotificationCreated { get; set; }
        public bool EmailSent { get; set; }

    }
}
