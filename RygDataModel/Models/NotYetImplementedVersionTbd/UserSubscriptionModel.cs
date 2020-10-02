using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// User notification subscriptions
    /// </summary>
    public class UserSubscriptionModel
    {

        public int Id { get; set; }
        public bool IsActive { get; set; }  //allows the user to temporarily de-activate a subscription, they can also remove it permanently by deleting the record

        /// <summary>
        /// SubscriptionType: (D)ata Entity; (R)egional 
        /// </summary>
        /// <remarks>
        /// Note that the data entities above will always have a GUID string Id, Regional will have a numeric Id, Country will have a string Id.
        /// </remarks>
        /// <example>
        /// SubscriptionType == RygDataModel.ModelHelper.StringSubsType(RygDataModel.SubsType.DataEntity)
        /// </example>
        public string SubscriptionType { get; set; }

        public bool NotifyByEmail { get; set; }
        public bool PendingDoubleOptInConfirmation { get; set; }

        /// <summary>
        /// SubToDataEntityType: (I)ncident; civil s(E)rvant; instituti(O)n; (C)alendar event; (B)log post; o(F)ficial register; a(L)ert
        /// </summary>
        /// <remarks>
        /// Note that the data entities above will always have a GUID string Id.
        /// In the case of o(F)ficial register; a(L)ert; (C)alendar event; (B)log post allow additional filter on Regiontype.
        /// </remarks>
        /// <example>
        /// SubToDataEntityType == RygDataModel.ModelHelper.StringDbEntity(RygDataModel.DbEntity.Incident)
        /// </example>
        public string SubToDataEntityType { get; set; }
        public string SubToDataEntityId { get; set; }

        public bool PlanningToParticipate { get; set; }     //only relevant to scheduled calendar events

        /// <summary>
        /// SubToRegionType: (S)tateprovinceregion; county(A)rea; ci(T)ty
        /// </summary>
        /// <remarks>
        /// Note that the regional entities above will always have an int Id.
        /// In the case of o(F)ficial register; a(L)ert; (C)alendar event; (B)log post allow additional filter on Regiontype.
        /// </remarks>
        /// <example>
        /// SubToRegionType == RygDataModel.ModelHelper.StringDbEntity(RygDataModel.DbEntity.Incident)
        /// </example>
        public string SubToRegionType { get; set; }
        public int SubToRegionId { get; set; }
        public bool NotifyOnIncident { get; set; }
        public bool NotifyOnCalendarEvent { get; set; }
        public bool NotifyReminderCalendarEvent { get; set; }
        public bool NotifyRegisterEntry { get; set; }
        public bool NotifyAlert { get; set; }
        public bool EmailOnIncident { get; set; }
        public bool EmailOnCalendarEvent { get; set; }
        public bool EmailReminderCalendarEvent { get; set; }
        public bool EmailRegisterEntry { get; set; }
        public bool EmailAlert { get; set; }
        //TODO: Country-Level, will currently be disabled
        //TODO: Online, will currently be disabled

        public DateTime AddedDate { get; set; }

    }
}
