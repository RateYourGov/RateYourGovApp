using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// User Flagged Event Reason,  to be loaded from the localized json file
    /// </summary>
    public class FlagReasonModel
    {

        public int Id { get; set; }
        public string Reason { get; set; }
        public string ReasonTagInfo { get; set; }
        
        public bool RequiresSupplementaryNote { get; set; }

        public int Severity { get; set; } //From lowest being less severe to highest being most severe

    }
}
