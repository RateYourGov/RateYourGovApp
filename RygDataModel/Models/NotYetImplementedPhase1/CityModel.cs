using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// City for the application setting country, to be loaded from the localized json file
    /// </summary>
    /// <remarks>
    /// Contains country code so could be expanded to foreign countries if desired, 
    /// NB: Json File Should contain a blank/unspecified/unknown entry to allow for anonymity or in instances where it is unknown
    /// </remarks>
    public class CityModel
    {

        public int Id { get; set; }
        public string CountryIsoCode { get; set; }

        //TODO: How to handle loading values here from json if auto increment field and value not known from json?  
        //      Perhaps StateProvinceRegion/CountyArea/City should be defined in a single, COUNTRY/StateProvinceRegion/CountyArea/City nested json file?
        public int StateProvinceRegionId { get; set; }
        public int CountyAreaId { get; set; }

        public string CityCode { get; set; } //Input & Formatting rules are set in FieldInputRules table
        public string CityName { get; set; }

        public bool IsVisible { get; set; }


        //TODD: possible issue when numeric id values are null? - commenting out and will include where used
        //public string FullKeyValue => new StringBuilder($"{CountryIsoCode?.ToString()}.{StateProvinceRegionId.ToString()}.{CountyAreaId.ToString()}.{CityCode?.ToString()}").ToString();

    }
}
