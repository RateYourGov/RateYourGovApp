using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Resources available, created by anyone authorized to do so as defined in application settings.  
    /// </summary>
    public class ResourceModel
    {

        public string Id { get; set; }

        public String ResourceGroupHeading { get; set; }
        public string ResourceShortDescription { get; set; }
        public string ResourceDetailedDescription { get; set; }
        public string ResourceWebsiteUrl { get; set; }
        public string ResourceTelephone { get; set; }
        public string ResourceEmail { get; set; }

        public string AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// LinkedToDataEntityType: instituti(O)n; o(F)ficial register; incidentcategor(Y)
        /// </summary>
        /// <remarks>
        /// Note that the data entities above will always have a GUID string Id.
        /// </remarks>
        /// <example>
        /// LinkedToDataEntityType == RygDataModel.ModelHelper.StringDbEntity(RygDataModel.DbEntity.Incident)
        /// </example>
        public string LinkedToDataEntityType { get; set; }
        public string LinkedToDataEntityId { get; set; }

        public string CountryIsoCode { get; set; }      //null if not linked
        public int StateProvinceRegionId { get; set; }  //null if not linked
        public int CountyAreaId { get; set; }           //null if not linked
        public int CityId { get; set; }                 //null if not linked

        public DateTime ModeratedDate { get; set; }
        public string ModeratedByUserId { get; set; }
        public string ModeratorNote { get; set; }
        public int AssignedDisplayPrioritySeq { get; set; }     //Ascending sort within ResourceGroupHeading, Lower numbers will be shown first

        public bool IsVisible { get; set; }

    }
}
