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

        public DateTime RequestDate { get; set; }                   //UTC

        public string ConfirmationToken { get; set; }               //null=not required
        public bool ConfirmationEmailSent { get; set; }             //null=not sent; 0=not required; 1=sent
        public DateTime ConfirmationEmailSentDate { get; set; }     //UTC, null=not sent
        public DateTime ConfirmedEmailDate { get; set; }            //UTC, null=not confirmed

        public bool AvailableEmailSent { get; set; }                //null=not sent; 0=not required; 1=sent
        public DateTime AvailableEmailSentDate { get; set; }        //UTC, null=not sent
        public DateTime UserRegisteredDate { get; set; }            //UTC, null=not registered

    }
}
