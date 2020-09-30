using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Stores non-sensitive data telling the App how to use an API service providor), to be added to published package so that a service can be chosen during initialization, 
    /// new service providors should be added to the GitHub repository https://github.com/RateYourGov/RateYourGovApp directly 
    /// or by creating an issue containing all of the model details at https://github.com/RateYourGov/RateYourGovApp/issues to make it available to future users/developers 
    /// as per the AGPL-3.0 License https://github.com/RateYourGov/RateYourGovApp/blob/main/LICENSE
    /// </summary>
    /// <remarks>
    /// Sensitive Authentication information like Token/User/Password should be stored in the individual project appsettings file.  
    /// NB: Be sure to use appsettings.Development.json or VS Secrets Store on your local macine to store your own authentication information so that it is not accidentally upload to GitHub.
    /// </remarks>
    public class IpApiServiceProviderModel
    {

        public string Id { get; set; }

        /// <summary>
        /// Required.
        /// </summary>
        public string ServiceProviderName { get; set; }

        /// <summary>
        /// Required.
        /// </summary>
        public string ServiceProviderWebsiteUrl { get; set; }

        /// <summary>
        /// Optional but useful, provide if possible.
        /// </summary>
        public string ServiceProviderDevInfoUrl { get; set; }

        /// <summary>
        /// authentication(T)oken; (U)serpassword
        /// </summary>
        /// <example>
        /// authType == RygDataModel.Helper.StringApiAuthType(RygDataModel.ApiAuthType.AuthenticationToken)
        /// </example>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// API URL.  Required.
        /// Replace sensitive data with placeHolders {AuthenticationToken}; {UserName}; {Password}; etc. dependent on AuthenticationType.
        /// </summary>
        public string ServiceProviderApiUrl { get; set; }
        public string HttpMethod { get; set; }

        /// <summary>
        /// (J)son; (X)ml; (T)ext.
        /// </summary>
        /// <example>
        /// dataType == RygDataModel.Helper.StringReadDataType(RygDataModel.ReadDataType.Json).
        /// </example>
        public string ResponseDataFormat { get; set; }

        /// <summary>
        /// Response Data Field Names. At least ONE of these is required, in order of preference: CountryIso3166_1_alpha_2_CodeFieldName, CountryIso3166_1_alpha_3_CodeFieldName, CountryNameFieldName.
        /// </summary>
        public string CountryIso3166_1_alpha_2_CodeFieldName { get; set; }     //Preffered/Required if available, Optional with CountryIso3166_1_alpha_3_CodeFieldName or CountryNameFieldName Required if not available
        public string CountryIso3166_1_alpha_3_CodeFieldName { get; set; }     //Preffered/Required if available and CountryIso3166_1_alpha_2_CodeFieldName is not, Optional with CountryNameFieldName Required if not available
        public string CountryNameFieldName { get; set; }                       //if ISO code is not provided then Required, else Optional
        /// <summary>
        /// Response Data Field Names. Optional but useful if available using the cheapest plan: TimeZoneFieldName; TimeZoneOffsetToZone with offset to GMT/UTC.
        /// </summary>
        public string TimeZoneFieldName { get; set; }                          //Optional
        public string TimeZoneOffsetFieldName { get; set; }                    //Optional
        public string TimeZoneOffsetToZoneIsFieldName { get; set; }            //Add actual timezone to TimeZoneOffsetToZone if false
        public string TimeZoneOffsetToZone { get; set; }                       //e.g. GMT/UTC/etc. or Fieldname if TimeZoneOffsetToZoneIsFieldName is true.  Required if TimeZoneOffsetFieldName is provided

        /// <summary>
        /// The template to use to extract information, preferred anyway , but optional if return data type supports field names above, required if not.   
        /// replace data location with {CountryIso3166_1_alpha_2_CodeFieldName}; {CountryIso3166_1_alpha_3_CodeFieldName}; {CountryNameFieldName}; {TimeZoneFieldName}; {TimeZoneOffsetFieldName} as available. 
        /// </summary>
        public string ResponseTextTemplate { get; set; }

        /// <summary>
        /// Provide if possible to make life easier for future developers.  
        /// Replace sensitive data with placeHolders like {AuthenticationToken}; {UserName}; {Password}; etc. or use the example provided by the Service Provider.
        /// </summary>
        public string ApiCallExample { get; set; }
        /// <summary>
        /// Provide if possible to make life easier for future developers, use the example provided by the Service Provider if it's easier.  
        /// </summary>
        public string ResponseTextExample { get; set; }

        public bool IsUnavailable { get; set; }
        public DateTime UnavailableSinceDate { get; set; }

        public DateTime AddedDate { get; set; }

    }
}
