using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Alert of illegal action currently underway by government actors.  Specific event that has a defined start and end date.  
    /// Be sure when using to check if a scheduled calendar event would not be more appropriate.
    /// </summary>
    /// <example>
    /// Police are arresting protesters at x location; Corrupt traffic cops are pulling cars over and ticketing unless a bribe is paid; etc.
    /// </example>
    public class AlertModel
    {

        public string Id { get; set; }

        public string AlertShortName { get; set; }
        public string AlertDetailsText { get; set; }
        public string AlertHashtags { get; set; }

        public string IncidentCategoryId { get; set; }

        public DateTime AlertStartDate { get; set; }
        public DateTime AlertEndDate { get; set; }
        public string TimezoneId { get; set; }
        public string CustomTimezoneText { get; set; }
        public double UtcOffsetMinutes { get; set; }
        public DateTime AlertStartServerDate { get; set; }
        public DateTime AlertEndServerDate { get; set; }
        public double ServerTimeOffsetMinutes { get; set; }

        //where applicable
        public string GeneralLocationInformationText { get; set; } //e.g. on the northbound section of the motorway between point x and y.  Typically used when a specific address is not involved.
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public int CityId { get; set; }
        public int CountyAreaId { get; set; }
        public int StateProvinceRegionId { get; set; }
        public string CountryIsoCode { get; set; }          //dependent on application settings
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //where applicable
        /// <summary>
        /// AlertLinkType: Links to UrlLinkType table on UrlLinkType.LinkTypeCharacter, see UrlLinkTypeModel. 
        /// </summary>
        /// <remarks>
        /// See UrlLinkTypeModel for use of the value to control logic in your code (should there be any) using RygDataModel.Helper.StringLinkType.
        /// </remarks>
        public string AlertLinkType { get; set; }
        public string DescriptiveText { get; set; }     //required when UrlLinkType.RequiresInputOfDescriptiveText
        public string AlertUrl { get; set; }

        public bool IsFlaggedForModeration { get; set; }
        public string FlaggedByUserId { get; set; }
        public string FlaggedForModerationText { get; set; }
        public DateTime FlaggedDate { get; set; }
        public DateTime ModeratorReviewDate { get; set; }
        public string ModeratorUserId { get; set; }
        public bool DeactivatedByModeratorAsFalseAlarm { get; set; }

        public string AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }
        public string ChangedByUserId { get; set; }
        public DateTime ChangedDate { get; set; }

        public bool NotificationCreated { get; set; }
        public bool EmailSent { get; set; }

        public bool AlertIsActive { get; set; }
        public DateTime DeactivatedDate { get; set; }
        public string DeactivatedByUserId { get; set; }
        public bool DeactivatedNotificationCreated { get; set; }
        public bool DeactivatedEmailSent { get; set; }

        public bool IsVisible { get; set; }

    }
}
