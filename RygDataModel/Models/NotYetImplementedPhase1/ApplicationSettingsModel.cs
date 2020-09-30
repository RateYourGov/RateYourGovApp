using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// RateYourGovApp Solution-wide application database models and helper that is non datadase server/view dependent.
/// </summary>
/// <remarks>
/// Naming Convention: Models should be named {TableName}Model and generally should by singular, e.g. UserModel instead of UsersModel.
/// Fields should have the same name as the database table field name, pay attention to maximum field name length and limit to the lowest common denominator allowed by the database, for now 64 characters (64=MySQL limit, even though MSSQL allows 128 characters). 
/// </remarks>
namespace RygDataModel.Models
{
    /// <summary>
    /// Non-Sensitive Application Settings.
    /// </summary>
    /// <remarks>
    /// Application/Server Level settings stored in the database to be available to all projects and avoid having to update duplicate settings in multiple locations.
    /// NB: Avoid adding any settings that are sensitive or impact application security here.
    /// NB: Some features associated with settings may not have been implemented yet and are here in anticipation of addition of the features in a future release 
    /// </remarks>
    public class ApplicationSettingsModel
    {

        public string Id { get; set; }                                     //Server Instance GUID, there will only be one record on file, but including for Dapper, best practice, etc. NB: THIS VALUE SHOULD NEVER BE CHANGED OR DATA WILL BE LOST!
        public string CountryIsoCode { get; set; }                         //Repository for entities in this country only, also used for globalization dependencies
        public string LanguageIsoCode { get; set; }                        //Repository Language used for globalization dependencies
        public int ServerTimeUtcOffsetMinutes { get; set; }                //Note that it's SERVER time, may need to change when moving hosts and, when used should inform users when appropriate 
        public string SiteName { get; set; }                               //Site name in localized language, used on pages and html page headers displayed in browser tabs
        public string SiteDescription { get; set; }                        //Site description in localized language, used in html page header metadata displayed by search engines
        public string WebsiteUrl { get; set; }                             //The main URL by which to access the website 

        public bool EnableReCaptcha { get; set; }                          //Enable reCAPTCHA on registration of email address functionality.  See: https://www.google.com/recaptcha/about/

        public bool IncludeRepositoryLinkOnPages { get; set; }             //Include a link to the open source repository on the web page(s) where relevant 
        public string RepositoryUrl { get; set; }                          //https://github.com/RateYourGov/RateYourGovApp
        public string RepositoryLinkLocalizedText { get; set; }            //Local server language Text to be shown for the link, can be html markup text - use {RepositoryUrl} as a placeholder to replace with the RepositoryUrl URL if Html

        public bool IncludeAboutLinkOnPages { get; set; }                  //Include a link to the about page on the web page(s) where relevant 
        public string AboutLinkLocalizedText { get; set; }                 //Local server language Text to be shown for the link

        public bool RegisterByInvitationOnly { get; set; }                 //Can be used to limit contributors to a closed group, but mostly used to limit registration during testing
        public bool AllowEmailToInvitee { get; set; }                      //If true, an email is sent to the invitee, else just add record and tell invitor to inform the user, setting will be ignored if RequireDoubleOptInOnEmailNotifyRequests is required. NB: Read the Terms and Conditions or local laws like GDPR carefully, if they require double opt-in, this should always be disabled.  TODO: perhaps use a mailto link to open local user email?
        public bool AdminCanInvitePeople { get; set; }                     //Administrators can invite people to join if RegisterByInvitationOnly, only useful if Users can't invite people, if none of these are set, only SuperAdministrators can invite people to join
        public bool ModeratorCanInvitePeople { get; set; }                 //Moderators can invite people to join if RegisterByInvitationOnly, only useful if Users can't invite people, if none of these are set, only SuperAdministrators can invite people to join
        public bool UserCanInvitePeople { get; set; }                      //Any user can invite people to join if RegisterByInvitationOnly, if none of these are set, only SuperAdministrators can invite people to join
        public int NoResponseInviteDays { get; set; }                      //Email Invitor User to inform them if the invitee has not responded after this many days, automatically delete the invitation within DeleteRegistrationInvitetDays dqys
        public int DeleteRegistrationInviteDays { get; set; }              //Delete the registration request record after this many days of informing the invitor user as per NoResponseInvitetDays if the invitee has not registered, or after this many days after they have registered 

        public bool ValidateCountryUsingIp { get; set; }                   //Enable functionality to validate and determine if a user is in-country ONLY WHEN REGISTERING using the user's IP address country by using an IP details API
        public string IpApiServiceProviderId { get; set; }                 //The IP API ServiceProvider to use based on providers defined in the IpApiServiceProvider table/json file
        public int MaxUserRegistationsPerMonth { get; set; }               //The maximum number of user registrations per month, used when your IP validation API service account has a monthly limi on calls based on your account
        public int CurrentMonthRegistationCount { get; set; }              //The number of user registrations so far this month (MAINTAINED BY THE APPLICATION)
        public DateTime CurrentMonthStartDate { get; set; }                //The start date of the current month (MAINTAINED BY THE APPLICATION)
        
        public bool AllowUserNotifyOnRegAvailable { get; set; }            //Enable functionality for someone to register their email address to be notified when registration becomes available (when they aren't able to register because the monthly limit has been reached, or registration is not available to them due to config settings) - also see RequireDoubleOptInOnEmailNotifyRequests setting which applies
        public int FailRegistrationRequestDays { get; set; }               //Email User to inform them if the request has not been able to be fulfilled after this many days, add a link to allow them to automatically renew the request within DeleteRegistrationRequestDays dqys
        public int DeleteRegistrationRequestDays { get; set; }             //Delete the registration request record after this many days of informing the user as per FailRegistrationRequestDays if they have not renewed, or after this many days after they have registered

        public bool UseProfilePictures { get; set; }                       //NULL=Use First Letter of User Name; 0=Use Avatars; 1=Allow upload and use of profile pictures
        public bool AnonimizeUserNameOnDelete { get; set; }                //NULL=Permanently delete all contributions; 0=Leave user data as is and set used blocked+deleted; 1=Anonimize, Perhaps @Charles becomes @Ch***es123 ? TBD TODO

        public bool AllowDifferentEntityCountry { get; set; }              //If enabled, will allow entry of entity data defined in AllowDifferentEntityCountryEntities for a foreign country
        public string AllowDifferentEntityCountryEntities { get; set; }    //Data Entities to which AllowDifferentEntityCountry applies, space separated - see RygDataModel.DbEntity.StringDbEntity

        public bool MaintenanceUsingService { get; set; }                  //1=Use the RygServiceApp to manage Create Notifications + Emails; monitor spam activity; system maintenance; etc, this is preffered if you are running on a hosted server instance or can install the service on a local pc that is always on and connected; 0=Manage through website, not ideal but use as a last resort if your budget does not allow for a service.  TODO: Determine strategy for how best to accomplish this...
        public int MaintenanceServiceRunFrequencyMinutes { get; set; }     //Run the service every x minutes, will be ignored if MaintenanceUsingAzureOrScheduler as you will need to set this up there
        public bool MaintenanceUsingAzureOrScheduler { get; set; }         //1=Use Scheduler like: Azure Functions (https://docs.microsoft.com/en-us/azure/azure-functions/); Task Scheduler; Cron Jobs to call API to manage Create Notifications + Emails; monitor spam activity; system maintenance; etc.  TODO: Determine strategy for how best to accomplish this...
        public int ScheduleCommentAutoFlagRule { get; set; }               //Perform function frequency in minutes: Run rules to automatically flag comments/block users 
        public int ScheduleCreateNotification { get; set; }                //Perform function frequency in minutes: Create Notification records
        public int ScheduleEmailNotification { get; set; }                 //Perform function frequency in minutes: Email Notifications, based on Digest rules where relevant
        public int ScheduleNotifyUsersPendingRegistration { get; set; }    //Perform function frequency in minutes: Clean up / Email Users Pending Registration, dependent on application settings  
        public int ScheduleCleanUpInvitedUsers { get; set; }               //Perform function frequency in minutes: Clean up / Email Invitor Users Pending Registration, dependent on application settings 
        public int ScheduleSystemBackup { get; set; }                      //Perform function frequency in minutes: Create Data/FTP/Copy/Send Backup based on defined strategy.  Set to -1 to never run if this is managed by the server or managed elsewhere e.g. using a tool like https://sqlbackupandftp.com/  
        public string ScheduleBackupStrategyTypeAndParms { get; set; }     //If ScheduleSystemBackup, Strategy type (First character) and parameters.  S=Create SQL generated data files?, etc.  TODO: Define strategies to be used 
        public int ScheduleSystemMaintenanceArchiving { get; set; }        //Perform function frequency in minutes: Archive/Clean up data 
        public int ScheduleCleanUpDiskFiles { get; set; }                  //Perform function frequency in minutes: Clean up unnecessary files/logs stored on server.  TODO: Define what that entails exactly. 
        public int ScheduleSystemMaintenance { get; set; }                 //Perform function frequency in minutes: Perform Other System Maintenance activities

        public int HashtagsTrendingCalculationDays { get; set; }           //The number of days to used by maintenance function to recalculate hashtag "used since" count, used to track "trending topics"
        public int DefaultExpireAlertDays { get; set; }                    //The default number of days to use to determine an Alert end date 
        public int MaxExpireAlertDays { get; set; }                        //The maximum number of days allow the creator to use for an Alert end date 

        public bool RequireDoubleOptInOnEmailNotifyRequests { get; set; }  //Enable functionality to send the user an email with a link to confirm notification additions before applying them, including notification of subscription available 
        public bool BccOptInEmailsSentToInternalAccount { get; set; }      //Enable blind copy of opt in/registration confirmation to the config.json internal email address as proof that it was sent store.  NB: This makes user emails visible to anyone with access to the bcc email address store, so a security risk.  try to avoid unless your host/regulations make it necessary to keep this level of detailed information.  
        public bool StoreOneWayHashBlockSpamReportEmailDays { get; set; }  //The number of days to store a unencryptable Hash of blocked deleted/email recipient reported as spam to allow blocking of attempted re-use of theemail address.  0=do not store
        public string OneWayServerSalt { get; set; }                       //Application generated server unique salt to be added to other unique data to be used as a key to generate one-way encrypted hash values for use/storage.  NB: THIS VALUE SHOULD NEVER BE CHANGED OR DATA WILL BE LOST!

        public int DefaultEmailNotifyDigestMinutes { get; set; }           //Use this setting to allow for notifications to be emailed in digest form rather than as they occur.  0=Send whenever the email service picks up the message, >0=create user digest every x minutes, e.g. 1440 will send a digest every 24 hours (overriden by user settings)
        public bool RequireDigestMinutesUsersettings { get; set; }         //Force the user to set EmailNotifyDigestMinutes
        public int UserMinimumNotifyDigestMinutes { get; set; }            //The minimum number of minutes to allow in user settings
        public bool EmailNotifyNewsletterInDigest { get; set; }            //0=Newsletters will be sent in their own email; 1=Newsletter link will be included in digest email
        public bool ExcludeNotificationsSeenFromDigest { get; set; }       //1=Do not include notifications that the user has already seen on the website in the digest.  This excludes Newsletter which will be sent regardless based on user opt-in settings

        public bool EnableEmailNotifyComments { get; set; }                //Enable functionality to send email notifications on Comments (Dependent on User Settings)
        public bool EnableEmailNotifyScheduleEvents { get; set; }          //Enable functionality to send email notifications for Schedule Events (Dependent on User Settings)
        public bool EnableEmailNotifySubbedEntities { get; set; }          //Enable functionality to send email notifications on Subscribed Entities (Dependent on User Settings)
        public bool EnableEmailNotifyNewsletter { get; set; }              //Enable functionality to send email of Newsletter (Dependent on User Settings)
        public bool EnableEmailNotifyBlogs { get; set; }                   //Enable functionality to send email of Blogs based on user subscriptions (Dependent on User Settings)
        public bool EnableEmailNotifyAlerts { get; set; }                  //Enable functionality to send email of Alerts based on user subscriptions (Dependent on User Settings)
        public bool EnableEmailNotifyNbAccountUpdates { get; set; }        //Enable functionality to send email notifications on NB Account Updates (Dependent on User Settings)
        public bool EnableEmailNotifyAdminToDo { get; set; }               //Enable functionality to send email notifications to Administrators when an item is added to the AdminToDoItem list (Dependent on User Settings)
        public bool EnableEmailNotifyAdminToDoModerator { get; set; }      //Enable functionality to send email notifications to Administrators when an item is added to the AdminToDoItem list marked as Moderator Action (Dependent on User Settings)
        public bool EnableEmailNotifyIncidentIncomplete { get; set; }      //Enable functionality to send email reminding a user of incomplete incident reports (Dependent on User Settings)
        public int RemindIncidentIncompleteDays { get; set; }              //Number of days after incident create incomplete to remind user through notifications and email (Dependent on EnableEmailNotifyIncidentIncomplete and User Settings)
        public int DeleteIncidentIncompleteDays { get; set; }              //Number of days after incident create incomplete to delete the incident data
        public int PauseUserEmailAfterInactiveDays { get; set; }           //Pause emails to a user if they have not signed in for this many days 
        public bool EnableEmailReminderScheduleEvents { get; set; }        //Enable functionality to send email reminder notifications for Schedule Events (Dependent on User Settings)
        public int RemindScheduleEventMinutes { get; set; }                //-1=Don't Remind, Number minutes before Scheduled Calendar Event to create notifications and email reminders (Dependent on EnableEmailNotifyIncidentIncomplete and User Settings)

        public int ShowReadNotificationDays { get; set; }                  //Show read notifications for this many days after user has seen them 
        public int DeleteReadNotificationDays { get; set; }                //Delete read notification db records after this many days from when the user has seen them 

        public bool SaveArchiveDeletedDataToDiskFile { get; set; }         //Save data deleted from the db during archiving activities to files on the server filesystem

        public bool EnableGoogleGenericAds { get; set; }                   //Enable functionality to Show Google Ads 
        public bool EnableGoogleTargetedAds { get; set; }                  //Enable functionality to Show Google Targeted Ads (Dependent on User Settings)

        public bool EnablePayPalDonations  { get; set; }                   //Enable PayPal donation functionality 
        public bool EnableBitcoinDonations { get; set; }                   //Enable Bitcoin donation functionality 

        public bool AllowCommentOnIncidents { get; set; }                  //Enable comment functionality on Incidents
        public bool AllowCommentOnCivilServants { get; set; }              //Enable comment functionality on Civil Servants
        public bool AllowCommentOnInstitutions { get; set; }               //Enable comment functionality on Institutions
        public bool AllowCommentOnSchedules { get; set; }                  //Enable comment functionality on Scheduled Events
        public bool AllowCommentOnBlogs { get; set; }                      //Enable comment functionality on Blog Posts
        public bool AllowIncident { get; set; }                            //Enable contribututions (create institutions, civil servants and incidents)
        public bool AllowSchedule { get; set; }                            //Enable Scheduling functionality
        public bool AllowBlog { get; set; }                                //Enable Blog Post functionality 

        public bool AllowAlerts { get; set; }                              //Enable Alerts functionality 
        public string AllowAlertCategoryList { get; set; }                 //Data Entities for which alerts can be added, space separated - see RygDataModel.DbEntity.StringDbEntity
        public string AllowAdminAlertCategoryList { get; set; }            //Data Entities for which Administrators can add alerts, space separated - see RygDataModel.DbEntity.StringDbEntity
        public string AllowModeratorAlertCategoryList { get; set; }        //Data Entities for which Moderators can add alerts, space separated - see RygDataModel.DbEntity.StringDbEntity
        public string AllowUserAlertCategoryList { get; set; }             //Data Entities for which uses can add alerts, space separated - see RygDataModel.DbEntity.StringDbEntity

        public bool AllowUserComment { get; set; }                         //Enable comment functionality for in-country users
        public bool AllowUserIncident { get; set; }                        //Enable contribututions (create institutions, civil servants and incidents) for in-country users
        public bool AllowUserSchedule { get; set; }                        //Enable Scheduling functionality for in-country users
        public bool AllowUserBlog { get; set; }                            //Enable Blog Post functionality for in-country users 
        public bool AllowUserAlert { get; set; }                           //Enable Alert Post functionality for in-country users 

        public bool AdminUserSchedule { get; set; }                        //Enable Scheduling functionality for Administrators, useful only when disabled for users
        public bool AdminUserBlog { get; set; }                            //Enable Blog Post functionality for Administrators, useful only when disabled for users

        public bool ModeratorUserSchedule { get; set; }                    //Enable Scheduling functionality for Moderators, useful only when disabled for users
        public bool ModeratorUserBlog { get; set; }                        //Enable Blog Post functionality for Moderator, useful only when disabled for users

        public bool AllowOutOfCountryUserRegistration { get; set; }        //Enable ability for users from other countries to register, only useful if at least one of the foreign user functions is enabled
        public bool AllowForeignUserComment { get; set; }                  //Enable comment functionality for users from other countries
        public bool AllowForeignUserIncident { get; set; }                 //Enable contribututions (create institutions, civil servants and incidents) for users from other countries
        public bool AllowForeignUserSchedule { get; set; }                 //Enable Scheduling functionality for users from other countries
        public bool AllowForeignUserBlog { get; set; }                     //Enable Blog Post functionality for users from other countries 
        public bool AllowForeignUserAlert { get; set; }                    //Enable Alert Post functionality for users from other countries

        public bool AllowAdminAddIncidentCategory { get; set; }            //Allow Administrators to add incident categories, useful only when disabled for users
        public bool AllowModeratorAddIncidentCategory { get; set; }        //Allow Moderators to add incident categories, useful only when disabled for users
        public bool AllowUserAddIncidentCategory { get; set; }             //Allow Users to add incident categories   
        public bool AllowAdminAddResource { get; set; }                    //Allow Administrators to add resources to ResourceModel, useful only when disabled for users   
        public bool AllowModeratorAddResource { get; set; }                //Allow Moderators to add resources to ResourceModel, useful only when disabled for users   
        public bool AllowUserAddResource { get; set; }                     //Allow Users to add resources to ResourceModel, useful only when disabled for users   
        public bool AllowAdminAddFaq { get; set; }                         //Allow Administrators to add FAQ to FaqModel, useful only when disabled for users   
        public bool AllowModeratorAddFaq { get; set; }                     //Allow Moderators to add FAQ to FaqModel, useful only when disabled for users   

        public bool RequireRevewGenerics { get; set; }                     //Require Review of Generic Institutions/Alerts/Blogs/Calender before Notifications. Alerts/Blogs/Calender are determined to be generic based on their Country/StateProvinceRegion/CountyArea/City "applies to location" level   
        public bool RequireRevewGenericEntities { get; set; }              //Data Entities for which Generic reviews is required, space separated and limited to entities above where functionality exists - see RygDataModel.DbEntity.StringDbEntity 
        public bool GenericLocationEntities { get; set; }                  //Data Entities that are required to make Alerts/Blogs/Calender Non-Generic, space separated and limited to Country/StateProvinceRegion/CountyArea/City entities where functionality exists - see RygDataModel.DbEntity.StringDbEntity 
        public bool AdminRevewGenericEntities { get; set; }                //Data Entities which Generic reviews need to be done by Administrators, space separated and limited to entities above where functionality exists - see RygDataModel.DbEntity.StringDbEntity 
        public bool ModeratorRevewGenericEntities { get; set; }            //Data Entities which Generic reviews can to be done by Moderators, space separated and limited to entities above where functionality exists - see RygDataModel.DbEntity.StringDbEntity 

        public int FlagCountForHidingComment { get; set; }                 //The number of times a comment needs to be flagged before it is hidden automatically, usually 1 
        public int FlagCountForTimeoutUser { get; set; }                   //The number total (for all) comment flags that a user needs to receive in the defined period before a user is automatically timed out by the system 
        public int CountMinutesForTimeoutUser { get; set; }                //The defined period referred to above
        public int FlagCountForBlockingUser { get; set; }                  //The number total (for all) comment flags that a user needs to receive in the defined period before a user is automatically blocked by the system 
        public int CountMinutesForBlockingUser { get; set; }               //The defined period referred to above
        public int DefaultTimeoutMinutes { get; set; }                     //The default number of minutes that a user will be timed out for, when timed out by a moderator they can override the timeout duration 

        public int CommentFetchLimit { get; set; }                         //The number of thread or reply comments to fetch at a time (MSSQL: TOP @CommentFetchLimit, MySQL/MariaDB: LIMIT @CommentFetchLimit) for UI paging

        public bool InitializationSetupComplete { get; set; }              //null=not started/new instance; 0=in progress; 1=complete

        // Use FieldInputRules to control instead     public bool EnableSalaryInformation { get; set; }                  //Enable entry of last known civil servant salary information functionality (in countries where the information is publicly available through, for example FOIA request)

    }
}
