using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Website URL links attached to IncidentModel or IncidentUpdateModel.
    /// </summary>
    public class IncidentLinkModel
    {

        public int Id { get; set; }

        /// <summary>
        /// ForEntityType: (I)ncident; incident u(P)date.  
        /// </summary>
        /// <remarks>
        /// Note that the data entities above will always have a GUID string Id.
        /// </remarks>
        /// <example>
        /// forEntityType == RygDataModel.Helper.StringDbEntity(RygDataModel.Helper.DbEntity.Incident)
        /// </example>
        public string ForEntityType { get; set; }
        public string ForEntityId { get; set; }

        /// <summary>
        /// IncidentLinkType: Links to UrlLinkType table on UrlLinkType.LinkTypeCharacter, see UrlLinkTypeModel. 
        /// </summary>
        /// <remarks>
        /// See UrlLinkTypeModel for use of the value to control logic in your code (should there be any) using RygDataModel.Helper.StringLinkType.
        /// </remarks>
        public string IncidentLinkType { get; set; }
        public string DescriptiveText { get; set; }     //required when UrlLinkType.RequiresInputOfDescriptiveText

        public string LinkedUrl { get; set; }
        public int DisplaySequence { get; set; }        //allows the user to change the order in which links are displayed (DisplaySequence, Id ascending), not unique, default to 0 

    }
}
