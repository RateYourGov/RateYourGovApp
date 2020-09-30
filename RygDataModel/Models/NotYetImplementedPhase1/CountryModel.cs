using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// ISO 3166-1 alpha-2 / alpha-3 Country to be loaded from the localized json file
    /// </summary>
    public class CountryModel
    {
        /// <summary>
        /// ISO 3166-1 alpha-2 country code
        /// </summary>
        public string CountryIsoCode { get; set; }
        
        public string Iso3166_1_alpha_3_Code { get; set; }      //ISO 3166-1 alpha-3 country code, needed in case this is what the IP API lookup service returns
        public string EnglishShortName { get; set; }            //keep English name for International users?

        public string LocalizedName { get; set; }

        public bool IsVisible { get; set; }

    }
}
