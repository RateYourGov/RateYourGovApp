using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// ISO 6391 Language to be loaded from the localized json file
    /// </summary>
    public class LanguageModel
    {

        public string LanguageIsoCode { get; set; }
        public string LanguageIso639_2_5Code { get; set; }
        public string LanguageIso639_3Code { get; set; }
        public string LanguageIso639_1Code { get; set; }
        public string EnglishName { get; set; }             //keep English name for International users?
        public string LocalizedName { get; set; }
        public string LanguageScope { get; set; }
        public string LanguageType { get; set; }
        public bool IsBibliographic { get; set; }

        public bool IsVisible { get; set; }
        
    }
}
