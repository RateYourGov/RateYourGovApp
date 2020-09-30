using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Timezone for the application setting country, to be loaded from the localized json file
    /// </summary>
    /// <remarks>
    /// Contains country code so could be expanded to foreign countries if desired, defaults to UTC if not found
    /// </remarks>
    public class TimezoneModel
    {

        public int Id { get; set; }
        public string CountryIsoCode { get; set; }
        public string TimeZoneCode { get; set; }
        public string TimezoneDescription { get; set; }

        //note that this is the offset in relation to the HOSTING SERVER, not UTC/GMT, 
        //it makes calculation of user time for schedule views, etc. easier as we don't need to understand the complexities of Daylight Savings Time or government changes to timezones.  
        //The downside is that the globalization file will need to be customized and potentially changed when switching to another host.
        //It also multiplies the work if multiple countries are loaded.
        //This could be made easier by providing an admin function to change the offset in bulk by adding/subtracting specified minutes.
        //This would also allow for the globalization file to be standardized with UTC offset and adding this function in the initialization wizard. 
        //This comment is turning into a novella, kudos to you if you got this far and apologies for the lack of a TLDR :)
        public int ServerTimeOffsetMinutes { get; set; }

        public bool IsVisible { get; set; }

        //In case it is needed for future functionality
        public string StandardizedTimezoneCode { get; set; }         //standardized abbreviated code of timezone, see: https://www.timetemperature.com/abbreviations/world_time_zone_abbreviations.shtml
        public string StandardizedTimezoneName { get; set; }         //standardized name of timezone, see: https://www.timetemperature.com/abbreviations/world_time_zone_abbreviations.shtml
        public double StandardizedTimezoneGmtOffset { get; set; }    //standardized name of timezone, see: https://www.timetemperature.com/abbreviations/world_time_zone_abbreviations.shtml
        public string StandardizedTzTimezoneCode { get; set; }       //standardized TZ Database Name of timezone, typically returned by IP lookup services, see: https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
        public double StandardizedTzTimezoneUtcOffset { get; set; }  //standardized TZ Database UTC offset, typically returned by IP lookup services, see: https://en.wikipedia.org/wiki/List_of_tz_database_time_zones

        public string FullKeyValue => new StringBuilder($"{CountryIsoCode?.ToString()}.{TimeZoneCode?.ToString()}").ToString();

    }
}
