using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Incident Details.  Some field are stored which aren't necessary for eventual use, but to track and store data through the issue creation process.  
    /// </summary>
    /// <remarks>
    /// The idea is to use the browser local store combined with the db to save data so that a user can come back to creating an incident and the system can remind them to do so.
    /// </remarks>
    public class IncidentModel
    {

        /// <summary>
        /// GUID unique to the Incident 
        /// </summary>
        public string Id { get; set; }

        public string InstitutionId { get; set; }
        public bool IncidentInvolvesPerson { get; set; }
        public string PersonId { get; set; }

        public string IncidentCategoryId { get; set; }
        public bool PositiveIncident { get; set; }

        public string InformationSourceId { get; set; }
        public string InformationSourceRelation { get; set; }
        public string NonFirstHandReason { get; set; }  //if the information is not first hand, give a reason why you are authorized/need or have reliable sources to report the incident

        public bool IncidentOccurredOnInstitutionPremises { get; set; }  //not relevant to generic institutions
        public string IncidentAddress1 { get; set; }                
        public string IncidentAddress2 { get; set; }                
        public string IncidentAddress3 { get; set; }                
        public string IncidentAddress4 { get; set; }                
        public string IncidentAddress5 { get; set; }                
        public int IncidentCityId { get; set; }                     
        public int IncidentCountyAreaId { get; set; }               
        public int IncidentStateProvinceRegionId { get; set; }      
        public string IncidentCountryIsoCode { get; set; }              //read-only or null dependent on application settings
        public double IncidentLatitude { get; set; }
        public double IncidentLongitude { get; set; }

        public int IncidentYear { get; set; }
        public int IncidentMonth { get; set; }
        public int IncidentDay { get; set; }
        public string IncidentHeading { get; set; }
        public string IncidentDetailsText { get; set; }

        public bool IncidentRelatedToAnotherIncident { get; set; }
        public bool RelatedIncidentReportedBySameUserId { get; set; }
        public string RelatedIncidentId { get; set; }

        public bool IncidentClosed { get; set; }
        public bool SatisfactoryResolution { get; set; }
        public bool CoveredByNewsMedia { get; set; }
        public string IncidentStatusId { get; set; }                //Current status and resolved reasons
        public string ResolvedByInstitutionId { get; set; }         //Bear in mind that "resolved" can be both satisfactory (justice) OR unsatisfactory (injustice)
        public string ResolvedByPersonId { get; set; }              //Bear in mind that "resolved" can be both satisfactory (justice) OR unsatisfactory (injustice)
        public string OtherStatusDescription { get; set; }
        public DateTime LastStatusUpdateDate { get; set; }
        public int IncidentStatusUpdateCount { get; set; }

        public bool PersonAddedToOfficialRegisterResult { get; set; }
        public string OfficialRegisterEntryId { get; set; }

        public bool CreateProcessComplete { get; set; }
        public int CurrentCreateStep { get; set; }
        public bool IncompleteNotificationCreated { get; set; }
        public bool IncompleteReminderEmailSent { get; set; }
        public DateTime IncompleteReminderEmailSentDate { get; set; }

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
