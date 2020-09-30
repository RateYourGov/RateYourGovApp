using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Users invited to join the website, used if application setting RegisterByInvitationOnly.  
    /// NB: Inform the invitor that the invitee won't be able to register (based on application settings) and require that they tick a box confirming the invitee country is the same as the application setting.
    /// </summary>
    public class InvitedUserModel
    {

        public int Id { get; set; }

        public string InvitorId { get; set; }
        public string EncryptedInvitorNameToUse { get; set; }
        public bool AllowIncludeInvitorEmail { get; set; }

        public string EncryptedInviteeEmail { get; set; }

        public DateTime InvitationDate { get; set; }
        public bool EmailSent { get; set; }
        public DateTime EmailSentDate { get; set; }

        public bool NoResponseEmailSent { get; set; }
        public DateTime NoResponseEmailSentDate { get; set; }

        public DateTime InviteeRegisteredDate { get; set; }

    }
}
