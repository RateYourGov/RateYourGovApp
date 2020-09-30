using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// The source of the incident information being reported,  to be loaded from the localized json file
    /// </summary>
    public class InformationSourceModel
    {

        public string Id { get; set; }
        public string InformationSourceName { get; set; }
        public string InformationSourceTagInfo { get; set; }
        public bool IsFirstHand { get; set; }
        public bool IsOfficialRegister { get; set; }
        public bool OfficialRegisterId { get; set; }

        /// <summary>
        /// The level of trust associated with the information source from 1 being least reliable to 3 being most reliable.
        /// </summary>
        public int TrustRating { get; set; }

    }
}
