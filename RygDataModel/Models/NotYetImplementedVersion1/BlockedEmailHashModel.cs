using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// A one way hash encryption of email address blocked from registration stored for the period 
    /// defined in application setting StoreOneWayHashBlockSpamReportEmailDays.
    /// Deleted after StoreOneWayHashBlockSpamReportEmailDays days.
    /// </summary>
    public class BlockedEmailHashModel
    {

        public int Id { get; set; }

        public string BlockedEmailHash { get; set; }
        public int FlagReasonId { get; set; }

        public DateTime AddedDate { get; set; }
        public string AddedByUserId { get; set; }

    }
}
