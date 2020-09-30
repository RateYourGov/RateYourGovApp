using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// User Flagged for (potential) violation of policies.
    /// </summary>
    /// <remarks>
    /// A flagged comment or action taken to censure a user by a moderator or application logic
    /// </remarks>
    public class UserFlaggedEventModel
    {

        public int Id { get; set; }

        /// <summary>
        /// SubToDataEntityType: (I)ncident [guid]; (C)alendar event [guid]; (B)log post [guid]; official re[G]ister entry [guid]; a(L)ert [guid]; incident u(P)date [guid]; 
        /// comme(N)thread [int]; (R)eply [int]
        /// </summary>
        /// <remarks>
        /// <example>
        /// SubToDataEntityType == RygDataModel.Helper.StringDbEntity(RygDataModel.Helper.DbEntity.Incident)
        /// </example>
        public string FlaggedEntityType { get; set; }
        public string FlaggedEntityGuid { get; set; }
        public int FlaggedEntityId { get; set; }

        public int FlagReasonId { get; set; }
        public string FlaggedByUserId { get; set; }
        public string SupplementaryNote { get; set; }
        public DateTime FlaggedDate { get; set; }
        
        public bool WasReviewed { get; set; }
        public bool FlagIsValid { get; set; }
        public int FlagActionTakenTypeId { get; set; }
        public DateTime ModeratorReviewDate { get; set; }
        public string ModeratorNote { get; set; }
        public string ModeratorUserId { get; set; }

    }
}
