using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// State/Province/Region for the application setting country, to be loaded from the localized json file
    /// </summary>
    /// <remarks>
    /// Contains country code so could be expanded to foreign countries if desired, 
    /// NB: Json File Should contain a blank/unspecified/unknown entry to allow for anonymity or in instances where it is unknown
    /// </remarks>
    public class StateProvinceRegionModel
    {

        public int Id { get; set; }
        public string CountryIsoCode { get; set; }
        
        public string StateProvinceRegionCode { get; set; } //Input & Formatting rules are set in FieldInputRules table
        public string StateProvinceRegionName { get; set; }

        //TODD: possible issue when numeric id values are null? - commenting out and will include where used
        //public string FullKeyValue => new StringBuilder($"{CountryIsoCode?.ToString()}.{StateProvinceRegionCode?.ToString()}").ToString();

    }
}
