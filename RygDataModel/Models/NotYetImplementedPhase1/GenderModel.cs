using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Gender  to be loaded from the localized json file 
    /// </summary>
    public class GenderModel
    {

        public int Id { get; set; }
        public string GenderName { get; set; }
        public string GenderTagInfo { get; set; }

    }
}
