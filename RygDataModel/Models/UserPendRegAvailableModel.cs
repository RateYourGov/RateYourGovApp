using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// User has requested to be notified by email when registration becomes available.
    /// </summary>
    /// <remarks>
    /// Users that have not been able to register, either because of app settings or because we have exceeded the monthly registration limit.  
    /// Also used by the under construction website to record a list of users interested in being notified when the website is up and running.
    /// </remarks>
    public class UserPendRegAvailableModel
    {

        public string Id { get; set; }
        public string EncryptedEmailAddress { get; set; }
        public string EncryptedCountryIsoCode { get; set; }

        /// <summary>
        /// The reason that registration was not possible: (L)imit exceeded; registerby(I)nvitationonly; allowoutof(C)ountryuserregistration; (U)nder construction 
        /// </summary>
        /// <example>
        /// denyReason == RygDataModel.ModelHelper.StringRegDenyType(RygDataModel.RegistrationDenyReason.LimitExceeded)
        /// </example>
        public string RegistrationDeniedReason { get; set; }

        public DateTime RequestDate { get; set; }

        public string ConfirmationToken { get; set; }
        public bool ConfirmationEmailSent { get; set; }
        public DateTime ConfirmationEmailSentDate { get; set; }
        public DateTime ConfirmedEmailDate { get; set; }            //null=not confirmed

        public bool AvailableEmailSent { get; set; }
        public DateTime AvailableEmailSentDate { get; set; }
        public DateTime UserRegisteredDate { get; set; }            //null=not registered

    }
}
