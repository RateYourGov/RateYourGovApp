using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Scheduled Calendar Event
    /// </summary>
    /// <example>
    /// Planned protest; livestream to discuss or provide "news" coverage of a topic/event; 
    /// The city/legislature/etc. is holding a meeting to get feedback/debate/introduce a new law contrary to basic rights etc.
    /// </example>
    public class CalendarEventModel
    {

        public string Id { get; set; }

        /// <summary>
        /// EventTypeCharacter: Links to CalendarEventType table on CalendarEventType.LinkTypeCharacter, see CalendarEventTypeModel.CalendarEventTypeCharacter. 
        /// </summary>
        /// <remarks>
        /// See CalendarEventTypeModel for use of the value to control logic in your code (should there be any) using RygDataModel.ModelHelper.StringCalendarEventType.
        /// </remarks>
        public string EventTypeCharacter { get; set; }

        public string EventShortName { get; set; }
        public string EventDetailsText { get; set; }
        public string EventHashtags { get; set; }

        /// <summary>
        /// RelatedEntityType: (I)ncident; instituti(O)n; (B)log post; a(L)ert; civil s(E)rvant.  
        /// </summary>
        /// <example>
        /// forEntityType == RygDataModel.ModelHelper.StringDbEntity(RygDataModel.DbEntity.Incident)
        /// </example>
        public string RelatedEntityType { get; set; }       //null=not related
        public string RelatedEntityGuid { get; set; }       //null=not related
        public int RelatedEntityId { get; set; }            //null=not related

        public string IncidentCategoryId { get; set; }

        public int UsersPlanningToParticipateCount { get; set; }

        public DateTime ScheduledDate { get; set; }
        public string TimezoneId { get; set; }
        public string CustomTimezoneText { get; set; }
        public double UtcOffsetMinutes { get; set; }
        public DateTime ScheduledServerDate { get; set; }
        public double ServerTimeOffsetMinutes { get; set; }

        //where applicable
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
        /// EventLinkType: Links to UrlLinkType table on UrlLinkType.LinkTypeCharacter, see UrlLinkTypeModel. 
        /// </summary>
        /// <remarks>
        /// See UrlLinkTypeModel for use of the value to control logic in your code (should there be any) using RygDataModel.ModelHelper.StringLinkType.
        /// </remarks>
        public string EventLinkType { get; set; }
        public string DescriptiveText { get; set; }     //required when UrlLinkType.RequiresInputOfDescriptiveText
        public string EventUrl { get; set; }

        public bool IsFlaggedForModeration { get; set; }
        public string FlaggedByUserId { get; set; }
        public string FlaggedForModerationText { get; set; }
        public DateTime FlaggedDate { get; set; }
        public DateTime ModeratorReviewDate { get; set; }
        public string ModeratorUserId { get; set; }

        public string AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }
        public string ChangedByUserId { get; set; }
        public DateTime ChangedDate { get; set; }

        public bool NotificationCreated { get; set; }
        public bool EmailSent { get; set; }
        public bool ReminderNotificationCreated { get; set; }
        public bool ReminderEmailSent { get; set; }

        public bool IsVisible { get; set; }

    }
}
