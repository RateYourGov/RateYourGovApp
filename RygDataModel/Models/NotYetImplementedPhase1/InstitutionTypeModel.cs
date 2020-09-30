using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Institution Type (e.g. Police Department, Courthouse, VA facility etc.) to be loaded from the localized json file, but can be added to by users when creating institutions
    /// </summary>
    public class InstitutionTypeModel
    {

        public int Id { get; set; }
        public string InstitutionTypeName { get; set; }        
        public string InstitutionTypeTagInfo { get; set; }

    }
}
