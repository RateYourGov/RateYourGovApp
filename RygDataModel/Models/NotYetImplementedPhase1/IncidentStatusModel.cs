using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// The current status of the incident (TODO: Or Alert??)
    /// </summary>
    /// <example>
    /// Closed: I didn't take it further / In progress: Still dealing with it / In progress: Criminal Case / In progress: Civil Suit /  In progress: Petition /  In progress: Complaint Filed / 
    /// Closed: Criminal Conviction / Closed: Civil Suit Won / Closed: Civil Suit Lost / Closed: Added to Register / Closed: I gave Up
    /// </example>
    public class IncidentStatusModel
    {

        public string Id { get; set; }
        public string IncidentStatusName { get; set; }
        public string IncidentStatusTagInfo { get; set; }
        public bool CloseIncidentStatusType { get; set; }
        public bool SatisfactoryResolution { get; set; }
        public bool RequiresOtherStatusDescription { get; set; }

        //Can only be updated by administrators
        public bool DisplayOnSiteNavigationBar { get; set; }
        public string NavigationBarShortName { get; set; }
        public string NavigationBarHoverText { get; set; }

        public bool IsVisible { get; set; }

    }
}
