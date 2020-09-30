using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Non sensitive User Application Settings.
    /// </summary>
    /// <remarks>
    /// This model should contain NO PERSONAL SENSITIVE user data, which should be defined in .NET Core Identity ONLY to allow for easier GDPR compliance.
    /// These settings are defined in the Data Database for ease of access and to limit Bandwidth use by avoiding/reducing API calls/packet size.
    /// </remarks>
    public class UserSettingsModel
    {

        public string Id { get; set; }

        //Settings updated by the User on their Settings or Registration page of the website
        public string UserName { get; set; }                        //TODO: Should we update this to a generic value when the user is deleted to retain posts?  Perhaps @Charles becomes @Ch***es123 ? TBD
        public string CountryIsoCode { get; set; }
        public int InCountryTimezoneId { get; set; }
        public bool UsePicture { get; set; }                        //NULL=Use First Letter of User Name; 0=Use Avatars; 1= profile pictures, limited by UseProfilePictures application setting
        public string FirstLetterHtmlColorCode { get; set; }        //Application assigned random HTML background color code, eventually allow user to choose?
        public int AvatarId { get; set; }

        //TODO: Move to subscriptions 
        public bool NotifyCalendarEventsOnline { get; set; }        //e.g. Livestreams
        public bool NotifyCalendarEventsGlobal { get; set; }
        public bool NotifyCalendarEventsCountry { get; set; }
        public bool EmailNotifyCalendarEventsOnline { get; set; }   //e.g. Livestreams
        public bool EmailNotifyCalendarEventsGlobal { get; set; }
        public bool EmailNotifyCalendarEventsCountry { get; set; }

        public bool EmailNotifySubbedEntities { get; set; }
        public bool NotifyNewsletter { get; set; }
        public bool EmailNotifyNewsletter { get; set; }
        public bool EmailNotifyCommentReply { get; set; }
        public bool EmailNotifyCommentThread { get; set; }
        public bool EmailNotifyMentionInComment { get; set; }
        public bool EmailNotifyNbAccountUpdates { get; set; }
        public int EmailNotifyDigestMinutes { get; set; }           //Use this setting to allow for notifications to be emailed in digest form rather than as they occur.  0=Send whenever the email service picks up the message, >0=create user digest every x minutes, e.g. 1440 will send a digest every day.  A null value will default to the application settings.  may be required based on application setting.
        public DateTime LastDigestTime { get; set; }

        public bool AllowCookiesRequiredToFunction { get; set; }    //Block or Delete user and don't allow registration if not true
        public bool AllowCookiesAdvertising { get; set; }

        //Stored in the Identity Auth Roles DB for added security:
        // - IsSiteOwner
        // - IsSuperAdministrator

        //Settings updated by Administrators or Internally by the Application only
        public bool NotificationSuspended { get; set; }
        public bool IsAdministrator { get; set; }

        //Settings updated by Moderators, Administrators or Internally by the Application only
        public bool IsModerator { get; set; }
        public bool CanContribute { get; set; }                     //Leave NULL by default to use Application level setting
        public string LimitContributionToEntities { get; set; }     //Leave NULL by default to use Application level setting, space separated, see RygDataModel.DbEntity.StringDbEntity
        public bool CanComment { get; set; }                        //Leave NULL by default to use Application level setting
        public DateTime TimedOutExpiry { get; set; }
        public bool IsBlocked { get; set; }                         //TODO: See TODO above (UserName) and block the user automatically when deleted
        public bool IsDeleted { get; set; }                         //TODO: See TODO above (UserName) and set if the user automatically when deleted

        //Settings/Properties updated Internally by the Application only
        public int FlaggedCommentCount { get; set; }
        public int NotificationFailCount { get; set; }
        public int UnseenNotificationCount { get; set; }
        public DateTime NotificationsSeenDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public DateTime SettingsUpdatedDate { get; set; }
        public DateTime TermsAcceptedDate { get; set; }
        public string CreatedFromIpAddressCountryIsoCode { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
