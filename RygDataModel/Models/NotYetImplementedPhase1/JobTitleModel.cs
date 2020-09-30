using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Job titles for Civil Servants to be loaded from the localized json file, but can be added to by contributor users when creating Civil Servant Records
    /// </summary>
    public class JobTitleModel
    {

        public int Id { get; set; }
        public string JobTitleName { get; set; }
        public string JobTitleTagInfo { get; set; }
        public bool ElectedPosition { get; set; }

    }
}
