using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// List of telephone dialling codes/prefixes by Country
    /// </summary>
    public class CountryPhonePrefixModel
    {

        public int Id { get; set; }     //In case there is more than one per country, this application will use the first record found only
        public string CountryIsoCode { get; set; }
        public string PhonePrefix { get; set; }

        public bool IsVisible { get; set; }

    }
}
