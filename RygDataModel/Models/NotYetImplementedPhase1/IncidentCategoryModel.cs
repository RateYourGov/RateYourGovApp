using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// The general category of the incident, typically the category of the law involved (e.g. Disability Rights / Corruption / Racial Discrimination / etc.), loaded from localized json file.  
    /// Can be added to based on AllowAdminAddIncidentCategory/AllowModeratorAddIncidentCategory/AllowUserAddIncidentCategory application setting.
    /// </summary>
    public class IncidentCategoryModel
    {

        public string Id { get; set; }
        public string IncidentCategoryName { get; set; }
        public string IncidentCategoryTagInfo { get; set; }

        //Can only be updated by administrators
        public bool DisplayOnSiteNavigationBar { get; set; }
        public string NavigationBarShortName { get; set; }
        public string NavigationBarHoverText { get; set; }

        public bool IsVisible { get; set; }

    }
}
