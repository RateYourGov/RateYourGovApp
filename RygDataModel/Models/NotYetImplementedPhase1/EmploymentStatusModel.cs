using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Employment status indicating if the status denotes in active Service, to be loaded from the localized json file
    /// </summary>
    public class EmploymentStatusModel
    {

        public int Id { get; set; }
        public string EmploymentStatusName { get; set; }
        public string EmploymentStatusTagInfo { get; set; }
        public bool IsActiveService { get; set; }

    }
}
