using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// User notification entry including information on whether it was seen by the user and/or emailed.
    /// </summary>
    /// <remarks>
    /// Will be archived/deleted from the db dependent on application settings.
    /// </remarks>
    public class UserNotificationQueueModel
    {

        public int Id { get; set; }

        /// <summary>
        /// ForToDataEntityType:  
        ///(I)ncident [guid]; civil s(E)rvant [guid]; instituti(O)n [guid]; (C)alendar event [guid]; (B)log post [guid]; (S)tateprovinceregion [int]; 
        ///county(A)rea [int]; ci(T)ty [int]; comme(N)tthread [int]; (R)eply [int]; (M)ention [int]; o(F)ficial register [guid];  official re[G]ister entry [guid]; 
        ///a(L)ert [guid]; incidentcategor(Y) [guid]; co(U)ntry [iso]; u(Z)er [guid]; incident u(P)date [guid]
        /// </summary>
        /// <remarks>
        /// Records will be marked as read and deleted based on application settings.
        /// </remarks>
        /// <example>
        /// forToDataEntityType == RygDataModel.ModelHelper.StringDbEntity(RygDataModel.DbEntity.Incident)
        /// </example>
        public string ForToDataEntityType { get; set; }
        public string ForToDataEntityGuid { get; set; }
        public int ForToDataEntityId { get; set; }

        public string ForUserId { get; set; }
        public bool EmailRequired { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime SeenByUserDate { get; set; }
        public DateTime EmailSentDate { get; set; }

        public bool IsVisible { get; set; }

    }
}
