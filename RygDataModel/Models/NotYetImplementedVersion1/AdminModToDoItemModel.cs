using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Administrator/Moderator To Do List Item, usually notifications of messages posted from users or items flagged for maintenance by the system or users
    /// </summary>
    public class AdminModToDoItemModel
    {

        public int Id { get; set; }
        
        public int ToDoActionTypeId { get; set; }
        public DateTime CreateDate { get; set; }

        public bool ModeratorItem { get; set; }

        public bool AddedByUser { get; set; }
        public string FromUserId { get; set; }

        public string SubjectText { get; set; }
        public string MessageText { get; set; }

        /// <summary>
        /// (I)ncident; civil s(E)rvant; instituti(O)n; (C)alendar event; (B)log post; (S)tateprovinceregion; county(A)rea; ci(T)ty; comme(N)tthread; (R)eply; (M)ention; o(F)ficial register
        /// </summary>
        /// <example>
        /// LinkedItemType == RygDataModel.Helper.StringDbEntity(RygDataModel.Helper.DbEntity.Incident)
        /// </example>
        public string LinkedItemType { get; set; }
        public int LinkedItemId { get; set; }
        public string LinkedItemGuid { get; set; }

        public bool NotifyEmailRequired { get; set; }
        public bool NotifyEmailSent { get; set; }

        public bool ActionComplete { get; set; }
        public string CompletedByUserId { get; set; }
        public string CompletionComment { get; set; }
        public DateTime CompletedDate { get; set; }

    }
}
