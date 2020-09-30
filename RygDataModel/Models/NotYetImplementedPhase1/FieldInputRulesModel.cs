using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Database field validation rules, e.g. used for State/Province/Region for the application setting country, can be applied to any field, to be loaded from the localized json file.  
    /// Storing in the database so that we have a single place to define the rules, available to all projects and reduces the number of localization json files to maintain. 
    /// </summary>
    /// <remarks>
    /// Contains country code so could be expanded to foreign countries if desired, 
    /// NB: No rules will be applied if no entry is found for the field and some values will be ignored by the app if they do not apply to the field.
    /// </remarks>
    public class FieldInputRulesModel
    {

        public int Id { get; set; }
        public string CountryIsoCode { get; set; }

        /// <summary>
        /// Rules determined by hierarchical lookup, 
        /// starting at CountryIsoCode.DbFieldName.DbName.ViewName, 
        /// if not found, CountryIsoCode.DbFieldName.DbName, 
        /// if not found, CountryIsoCode.DbFieldName, 
        /// if not found, no rules applied
        /// </summary>
        public string DbFieldName { get; set; }         //Required
        public string DbTableName { get; set; }         //Optional
        public string ViewName { get; set; }            //Optional

        public string ShortNameText { get; set; }       //NULL for none, used for html table headings, etc. NB: remember to translate this to the server language in the json file 
        public string LabelText { get; set; }           //NULL for none, NB: remember to translate this to the server language in the json file 
        public string TagInfo { get; set; }             //NULL for none, NB: remember to translate this to the server language in the json file 
        public string InputInstructions { get; set; }   //NULL for none, TODO: add to tag info? How to handle for WAI-ARIA accessible compatibility?  NB: remember to translate this to the server language in the json file 
        public string InputHint { get; set; }           //NULL for none, NB: remember to translate this to the server language in the json file 
        public string DefaultValue { get; set; }        //NULL for none, NB: remember to translate this to the server language in the json file 

        public int MinLength { get; set; }              //0=Activates Not Required; >0 Will Make it a Required Field
        public int MaxLength { get; set; }
        public double MinValue { get; set; }            //Applies to Numeric field only
        public double MaxValue { get; set; }            //Applies to Numeric field only
        public string LimitToCharacters { get; set; }   //NULL for no limit, otherwise a list of characters to be allowed, NB: NO SPACES, use AllowSpaceCharacter to allow spaces
        public bool AllowSpaceCharacter { get; set; }  
        public bool ForceUppercase { get; set; }        //e.g. when entering state names or postal codes
        public bool ForceLowercase { get; set; }        //e.g. when entering ISO language codes
        public bool ForcePascalCase { get; set; }       //Every Word Starts With An Uppercase 
        public string InputMask { get; set; }           //NULL for no mask
        public bool HideField { get; set; }             //Hides the field in the UI where application does not require the data, this value will be ignored if the data is required by the app

        public string ViewKeyValue => new StringBuilder($"{CountryIsoCode?.ToString()}.{DbFieldName?.ToString()}.{DbTableName?.ToString()}.{ViewName?.ToString()}").ToString();
        public string DbKeyValue => new StringBuilder($"{CountryIsoCode?.ToString()}.{DbFieldName?.ToString()}.{DbTableName?.ToString()}").ToString();
        public string FieldKeyValue => new StringBuilder($"{CountryIsoCode?.ToString()}.{DbFieldName?.ToString()}").ToString();

    }
}
