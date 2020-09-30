using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Updates to the incident added by the orinial incident user to inform subscribers, update incident current status, add resolution details on closing 
    /// an incident, etc. 
    /// </summary>
    public class IncidentUpdateModel
    {

        public string Id { get; set; }

        public string IncidentId { get; set; }

        public string LinkedToNewIncidentId { get; set; }           //if the original incident lead to another incident, link the new incident here, TODO: what to populate IncidentUpdateHeading, IncidentUpdateDetailsText, etc. with
        public int IncidentUpdateYear { get; set; }
        public int IncidentUpdateMonth { get; set; }
        public int IncidentUpdateDay { get; set; }
        public string IncidentUpdateHeading { get; set; }
        public string IncidentUpdateDetailsText { get; set; }

        public bool IncidentClosedByThisUpdate { get; set; }
        public bool SatisfactoryResolution { get; set; }
        public bool CoveredByNewsMedia { get; set; }
        public string IncidentStatusId { get; set; }                //Current status and resolved reasons
        public string ResolvedByInstitutionId { get; set; }         //Bear in mind that "resolved" can be both satisfactory (justice) OR unsatisfactory (injustice)
        public string ResolvedByPersonId { get; set; }              //Bear in mind that "resolved" can be both satisfactory (justice) OR unsatisfactory (injustice)
        public string OtherStatusDescription { get; set; }

        public bool PersonAddedToOfficialRegisterResult { get; set; }
        public string OfficialRegisterEntryId { get; set; }

        public bool IsFlaggedForModeration { get; set; }
        public int FlaggedCount { get; set; }
        public DateTime LastFlaggedDate { get; set; }
        public DateTime ModeratorReviewDate { get; set; }
        public string ModeratorUserId { get; set; }
        public int FlagActionTakenTypeId { get; set; }
        public string ModeratorNote { get; set; }

        public string AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }
        public string ChangedByUserId { get; set; }
        public DateTime ChangedDate { get; set; }

        public bool NotificationCreated { get; set; }
        public bool EmailSent { get; set; }

        public bool IsVisible { get; set; }

    }
}
