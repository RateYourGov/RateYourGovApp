using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Official Misconduct/Commendation register to be loaded from the localized json file, but can be added to by contributor users when creating Civil Servant Records
    /// </summary>
    public class OfficialRegisterModel
    {

        public string Id { get; set; }
        public string RegisterName { get; set; }
        public string RegisterDescription { get; set; }
        public bool IsMisconductRegister { get; set; }  //0=Commendation; 1=Misconduct

        //Can only be updated by administrators
        public bool DisplayOnSiteNavigationBar { get; set; }
        public string NavigationBarShortName { get; set; }
        public string NavigationBarHoverText { get; set; }

        public bool IsVisible { get; set; }

    }
}
